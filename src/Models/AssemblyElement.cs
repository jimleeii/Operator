namespace Operator.Models;

/// <summary>
/// The assembly element.
/// </summary>
public record AssemblyElement
{
	/// <summary>
	/// Gets or sets the assembly path.
	/// </summary>
	public string? AssemblyPath { get; set; }

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the types.
	/// </summary>
	public TypeElement[]? Types { get; set; }
}