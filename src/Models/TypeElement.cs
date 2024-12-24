namespace Operator.Models;

/// <summary>
/// The type element.
/// </summary>
public class TypeElement
{
	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the methods.
	/// </summary>
	public Method[]? Methods { get; set; }
}