namespace Operator.EndpointDefinitions;

/// <summary>
/// The operator endpoint definition.
/// </summary>
public class OperatorEndpointDefinition : IEndpointDefinition
{
	/// <summary>
	/// Defines the endpoints.
	/// </summary>
	/// <param name="app">The app.</param>
	/// <remarks>
	/// Maps a GET endpoint to <c>api/Execute</c> that calls <see cref="ExecuteAsync"/>.
	/// </remarks>
	public void DefineEndpoints(WebApplication app)
	{
		app.MapGet("api/ListAssembles", ListAvaiableAssemblesAsync);
		app.MapGet("api/ListMethods", ListAvaiableMethodsAsync);
		app.MapPost("api/Execute", ExecuteAsync);
	}

	/// <summary>
	/// Defines the services.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <remarks>
	/// This method adds a singleton service for <see cref="IOperatorService"/> using <see cref="OperatorService"/> as the implementation.
	/// </remarks>
	public void DefineServices(IServiceCollection services)
	{
		services.AddScoped<IOperatorService, OperatorService>();
	}

	/// <summary>
	/// Lists all available assemblies that can be loaded.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <remarks>
	/// This method is invoked when a GET request is made to the <c>api/ListAssemblies</c> endpoint.
	/// </remarks>
	private static async Task<List<string>?> ListAvaiableAssemblesAsync(IOperatorService service)
	{
		await service.ConfigureServicesAsync();
		var assemblies = await service.ListAssembliesAsync();
		return assemblies;
	}

	/// <summary>
	/// Lists all available methods that can be invoked.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="assemblyName">The assembly name.</param>
	/// <remarks>
	/// This method is invoked when a GET request is made to the <c>api/ListMethods</c> endpoint.
	/// </remarks>
	/// <returns>A Task</returns>
	private static async Task<IList<AssemblyElement>?> ListAvaiableMethodsAsync(IOperatorService service, string assemblyName)
	{
		await service.ConfigureServicesAsync();
		var methods = await service.ListMethodsAsync(assemblyName);
		return methods;
	}

	/// <summary>
	/// Executes an asynchronous operation.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="request">The request.</param>
	/// <remarks>
	/// This method is invoked when a GET request is made to the <c>api/Execute</c> endpoint.
	/// </remarks>
	/// <returns>A Task of type object?</returns>
	private static async Task<object?> ExecuteAsync(IOperatorService service, ExecuteRequest request)
	{
		await service.ConfigureServicesAsync();
		var obj = await service.InvokeMethodAsync(request.AssemblyName, request.TypeName, request.MethodName, request.Parameters);
		return obj;
	}
}