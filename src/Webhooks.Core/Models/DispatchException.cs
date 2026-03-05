namespace Webhooks.Core;

/// <summary>
/// An exception thrown when webhook dispatch fails.
/// </summary>
public sealed class DispatchException(string message, DispatchExceptionKind kind, Exception? innerException = null)
    : Exception(message, innerException)
{
    /// <summary>
    /// Gets the category of dispatch failure.
    /// </summary>
    public DispatchExceptionKind Kind { get; } = kind;
}
