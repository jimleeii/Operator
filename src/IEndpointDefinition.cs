namespace Operator;

/// <summary>
/// The endpoint definition interface.
/// </summary>
public interface IEndpointDefinition
{
	/// <summary>
	/// Defines the services.
	/// </summary>
	/// <param name="services">The services.</param>
	void DefineServices(IServiceCollection services);

	/// <summary>
	/// Defines the endpoints.
	/// </summary>
	/// <param name="app">The app.</param>
	void DefineEndpoints(WebApplication app);
}