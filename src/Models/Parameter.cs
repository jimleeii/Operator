using System.Text.Json.Serialization;

namespace Operator.Models;

/// <summary>
/// The parameter.
/// </summary>
[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public sealed class Parameter
{
	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the type.
	/// </summary>
	public required string Type { get; set; }

	/// <summary>
	/// Gets or sets the value.
	/// </summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public object? Value { get; set; }
}