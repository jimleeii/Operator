using System.Text.Json.Serialization;

namespace Operator.DataContracts;

/// <summary>
/// The execute request.
/// </summary>
[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public sealed class ExecuteRequest
{
	/// <summary>
	/// Gets or sets the assembly name.
	/// </summary>
	public required string AssemblyName { get; set; }

	/// <summary>
	/// Gets or sets the type name.
	/// </summary>
	public required string TypeName { get; set; }

	/// <summary>
	/// Gets or sets the method name.
	/// </summary>
	public required string MethodName { get; set; }

	/// <summary>
	/// Gets or sets the parameter.
	/// </summary>
	public Parameter[]? Parameters { get; set; }
}