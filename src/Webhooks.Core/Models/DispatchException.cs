namespace Webhooks.Core;

public enum DispatchExceptionKind
{
    DispatcherUnavailable,
    MiddlewareFailure,
    InvocationFailure,
    ConfigurationError,
    Unknown
}

public class DispatchException(string message, DispatchExceptionKind kind, Exception? innerException = null)
    : Exception(message, innerException)
{
    public DispatchExceptionKind Kind { get; } = kind;
}
