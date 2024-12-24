namespace Operator.Exceptions;

/// <summary>
/// The type not found exception.
/// </summary>
public class TypeNotFoundException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TypeNotFoundException"/> class.
	/// </summary>
	/// <remarks>
	/// The default error message is "Cannot find type."
	/// </remarks>
	public TypeNotFoundException() : base("Cannot find type.")
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TypeNotFoundException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public TypeNotFoundException(string? message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TypeNotFoundException"/> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
	public TypeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}