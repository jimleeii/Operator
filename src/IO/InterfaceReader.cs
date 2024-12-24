using System.Reflection;
using System.Runtime.Loader;

namespace Operator.IO;

/// <summary>
/// The interface reader.
/// </summary>
public class InterfaceReader : IDisposable
{
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
				// TODO: dispose managed state (managed objects)
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
	~InterfaceReader()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: false);
	}

	/// <summary>
	/// Reads all the assemblies from all the interfaces in a DLL.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="dllPath">The dll path.</param>
	/// <returns>A list of assemblyelements.</returns>
	public List<AssemblyElement> ReadInterface(AssemblyLoadContext context, string dllPath)
	{
		// Get the types in the DLL
		Assembly assembly = context.LoadFromAssemblyPath(dllPath);
		var types = assembly.GetTypes();

		// Create a list to store the assemblies
		var assemblies = new List<AssemblyElement>();

		// Create a list to store the types
		var typeElements = new List<TypeElement>();

		// Iterate over the types
		foreach (var type in types)
		{
			// Check if the type is an interface
			if (type.IsInterface)
			{
				// Get the assemblies in the interface
				var interfaceMethods = type.GetMethods();

				// Create a list to store the methods
				var methods = new List<Method>();

				// Iterate over the assemblies
				foreach (var method in interfaceMethods)
				{
					// Get the method's name and parameters
					var methodName = method.Name;
					var parameters = method.GetParameters();

					// Create a list to store the parameters
					var parameterTypes = new List<Parameter>();

					// Iterate over the parameters
					foreach (var parameter in parameters)
					{
						// Get the parameter's name and type
						var parameterName = parameter.Name;
						var parameterType = parameter.ParameterType.FullName;

						// Add the parameter to the list
						parameterTypes.Add(new Parameter { Name = parameterName!, Type = parameterType! });
					}

					// Add the method to the list
					methods.Add(new Method { Name = methodName, Parameters = [.. parameterTypes] });

					// Add the type to the list
					typeElements.Add(new TypeElement { Name = type.FullName!, Methods = [.. methods] });

					// Add the method to the list
					assemblies.Add(new AssemblyElement { AssemblyPath = dllPath, Name = assembly.FullName!, Types = [.. typeElements] });
				}
			}
		}

		// Return the dictionary of assemblies
		return assemblies;
	}
}