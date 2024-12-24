namespace Operator.Services;

/// <summary>
/// The operator service interface.
/// </summary>
public interface IOperatorService
{
	/// <summary>
	/// Configures the services asynchronously.
	/// </summary>
	/// <returns>A Task</returns>
	Task ConfigureServicesAsync();

	/// <summary>
	/// List the assemblies asynchronously.
	/// </summary>
	/// <returns>A Task of a list of strings</returns>
	Task<List<string>> ListAssembliesAsync();

	/// <summary>
	/// List the methods asynchronously.
	/// </summary>
	/// <param name="assemblyName">The assembly name.</param>
	/// <returns>A Task of a list of assemblyelements</returns>
	Task<IList<AssemblyElement>> ListMethodsAsync(string assemblyName);

	/// <summary>
	/// Invokes the method asynchronously.
	/// </summary>
	/// <param name="assemblyName">The assembly name.</param>
	/// <param name="typeName">The type name.</param>
	/// <param name="methodName">The method name.</param>
	/// <param name="parameters">The parameters.</param>
	/// <returns>A Task of type object?</returns>
	Task<object?> InvokeMethodAsync(string assemblyName, string typeName, string methodName, params Parameter[]? parameters);
}