namespace Webhooks.Core;

/// <summary>
/// Middleware that participates in the broadcast pipeline before dispatch.
/// </summary>
public interface IBroadcastMiddleware
{
    /// <summary>
    /// Processes a broadcast invocation, optionally delegating to the next middleware.
    /// </summary>
    Task InvokeAsync(
        DeliveryEnvelope deliveryEnvelope,
        Func<DeliveryEnvelope, CancellationToken, Task> next,
        CancellationToken cancellationToken = default);
}
