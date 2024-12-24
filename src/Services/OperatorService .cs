using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.Loader;

namespace Operator.Services;

/// <summary>
/// The operator service.
/// </summary>
public class OperatorService(IOptions<AppSettings> appSettings) : IOperatorService, IDisposable
{
	/// <summary>
	/// Load context.
	/// </summary>
	private AssemblyLoadContext? _loadContext = AssemblyLoadContext.GetLoadContext(typeof(OperatorService).Assembly);

	/// <summary>
	/// Service provider.
	/// </summary>
	private IServiceProvider? _provider;

	/// <summary>
	/// Assembly list.
	/// </summary>
	private readonly List<AssemblyElement> _assemblies = [];

	/// <summary>
	/// The disposed value.
	/// </summary>
	private bool _disposed;

	/// <summary>
	/// Releases unmanaged and - optionally - managed resources.
	/// </summary>
	/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				// Dispose managed state (managed objects)
				if (_loadContext is not null)
				{
					if (_loadContext.IsCollectible)
					{
						_loadContext.Unload();
						_loadContext = null;
					}
				}

				_assemblies.Clear();
			}

			// TODO: free unmanaged resources (unmanaged objects) and override finalizer
			// TODO: set large fields to null
			_disposed = true;
		}
	}

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	// Finalizer is overridden in debug mode to track resource cleanup
	~OperatorService()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: false);
	}

	/// <summary>
	/// Loads services from assemblies in the 'Plugins' folder.
	/// </summary>
	/// <remarks>
	/// This method searches for assemblies in the 'Plugins' folder, and for each assembly, it searches for all interfaces and associated types.
	/// It then adds each associated type as a scoped service for the corresponding interface.
	/// </remarks>
	public Task ConfigureServicesAsync()
	{
		// Load plug-in folder from appsettings file
		string pluginsPath = appSettings.Value.PluginFolder ?? Path.Combine(AppContext.BaseDirectory, "Plugins");

		// Load plug-ins from a folder
		if (Directory.Exists(pluginsPath))
		{
			// Create new service collection
			ServiceCollection services = new();

			string[] dlls = Directory.GetFiles(pluginsPath, "*.dll");
			foreach (string dll in dlls)
			{
				Assembly assembly = _loadContext!.LoadFromAssemblyPath(dll);

				// Load all interfaces and associated types
				var interfaceTypes = assembly.GetTypes().Where(t => t.IsInterface).ToList();
				foreach (var interfaceType in interfaceTypes)
				{
					var implementingTypes = assembly.GetTypes().Where(t => interfaceType.IsAssignableFrom(t) && !t.IsInterface).ToList();
					foreach (var implementingType in implementingTypes)
					{
						// Check if interfaceType is already registered
						if (services.Any(s => s.ServiceType == interfaceType))
						{
							continue;
						}

						// Add the associated type as a scoped service
						services.AddScoped(interfaceType, implementingType);
					}
				}

				// Add assembly to the list
				_assemblies.Add(new AssemblyElement { AssemblyPath = dll, Name = assembly.FullName!, Types = [] });
			}

			// Build the provider
			_provider = services.BuildServiceProvider();
		}

		return Task.CompletedTask;
	}

	/// <summary>
	/// Lists all assembly names that are currently loaded.
	/// </summary>
	/// <returns>A Task that represents the asynchronous operation. The Task's result contains a list of assembly names.</returns>
	public Task<List<string>> ListAssembliesAsync()
	{
		return Task.FromResult(_assemblies.Select(a => a.Name).ToList());
	}

	/// <summary>
	/// Lists all available methods that can be invoked.
	/// </summary>
	/// <param name="assemblyName">The assembly name.</param>
	/// <returns>A Task of a list of assemblyelements</returns>
	public Task<IList<AssemblyElement>> ListMethodsAsync(string assemblyName)
	{
		using var interfaceReader = new InterfaceReader();
		var assembly = _assemblies.FirstOrDefault(a => a.Name.EqualsIgnoreCase(assemblyName));
		return Task.FromResult<IList<AssemblyElement>>(interfaceReader.ReadInterface(_loadContext!, assembly!.AssemblyPath!));
	}

	/// <summary>
	/// Invokes a method by name, with parameters.
	/// </summary>
	/// <param name="assemblyName">The assembly name.</param>
	/// <param name="typeName">The name of the type that contains the method.</param>
	/// <param name="methodName">The name of the method to invoke.</param>
	/// <param name="parameters">The parameters to pass to the method.</param>
	/// <returns>A Task that represents the asynchronous operation. The Task's result is the result of the method invocation.</returns>
	public Task<object?> InvokeMethodAsync(string assemblyName, string typeName, string methodName, params Parameter[]? parameters)
	{
		Assembly assembly = _loadContext!.LoadFromAssemblyPath(assemblyName);

		Type type = assembly.GetType(typeName) ?? throw new TypeNotFoundException($"Type {typeName} not found in assembly.");
		MethodInfo method = type.GetMethod(methodName) ?? throw new TypeNotFoundException($"Method {methodName} not found in type {typeName}.");

		object instance = _provider!.GetRequiredService(type);

		if (parameters == null)
		{
			return Task.FromResult(method.Invoke(instance, null));
		}

		List<object>? data = [];
		foreach (var parameter in parameters)
		{
			var methodParameters = method.GetParameters();
			var methodParameter = methodParameters.FirstOrDefault(param => parameter.Name.EqualsIgnoreCase(param.Name!));

			// Create a new instance of the type
			object? castedObj = Activator.CreateInstance(methodParameter!.ParameterType);

			if (castedObj == null)
			{
				continue;
			}

			if (parameter.Value != null)
			{
				castedObj = parameter.Value;
			}

			data.Add(castedObj);
		}

		return Task.FromResult(method.Invoke(instance, [.. data]));
	}
}