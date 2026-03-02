namespace Webhooks.Core;

public interface IBroadcastMiddleware
{
    Task InvokeAsync(
        DeliveryEnvelope deliveryEnvelope,
        Func<DeliveryEnvelope, CancellationToken, Task> next,
        CancellationToken cancellationToken = default);
}
