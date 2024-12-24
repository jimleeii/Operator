namespace Operator.Models;

/// <summary>
/// The method.
/// </summary>
public class Method
{
	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the parameters.
	/// </summary>
	public Parameter[]? Parameters { get; set; }
}
