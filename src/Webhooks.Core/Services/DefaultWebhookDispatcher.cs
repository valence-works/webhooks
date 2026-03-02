using Microsoft.Extensions.Logging;

namespace Webhooks.Core.Services;

public class DefaultWebhookDispatcher(
    IWebhookEndpointInvoker endpointInvoker,
    ILogger<DefaultWebhookDispatcher> logger) : IWebhookDispatcher
{
    public string Name => "default";

    public async Task<DispatchHandoffResult> DispatchAsync(
        DeliveryEnvelope deliveryEnvelope,
        WebhookSink webhookSink,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var outboundEvent = new NewWebhookEvent(
                deliveryEnvelope.EventType,
                deliveryEnvelope.Payload,
                deliveryEnvelope.EventId,
                deliveryEnvelope.DispatchTimestamp);
            await endpointInvoker.InvokeAsync(webhookSink, outboundEvent, cancellationToken);

            return new DispatchHandoffResult(Name, DispatchHandoffStatus.Accepted, deliveryEnvelope.EventId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Dispatcher handoff failed for sink {SinkId}", webhookSink.SinkId);
            return new DispatchHandoffResult(Name, DispatchHandoffStatus.Rejected, deliveryEnvelope.EventId, ex.Message);
        }
    }
}
