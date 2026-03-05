namespace Webhooks.Core;

/// <summary>
/// Identifies the category of a dispatch failure.
/// </summary>
public enum DispatchExceptionKind
{
    DispatcherUnavailable,
    MiddlewareFailure,
    InvocationFailure,
    ConfigurationError,
    Unknown
}
