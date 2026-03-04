using Microsoft.Extensions.Logging.Abstractions;
using Webhooks.Core.Options;
using Webhooks.Core.Services;

namespace Webhooks.Core.Tests.Dispatch;

public class PendingStatusTransitionTests
{
    [Fact]
    public async Task DispatchAsync_Preserves_Enqueued_Status_As_Pending_Transition()
    {
        var coordinator = new DispatcherInvocationCoordinator(
            new[] { new EnqueueDispatcher() },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions()),
            NullLogger<DispatcherInvocationCoordinator>.Instance);

        var result = await coordinator.DispatchAsync(
            new DeliveryEnvelope("evt-1", "order.created", null, DateTimeOffset.UtcNow),
            new WebhookSink { SinkId = "sink-a", Destination = new Uri("https://example.com/a") });

        Assert.Equal(DispatchHandoffStatus.Enqueued, result.HandoffStatus);
    }

    private sealed class EnqueueDispatcher : IWebhookDispatcher
    {
        public string Name => "default";

        public Task<DispatchHandoffResult> DispatchAsync(DeliveryEnvelope deliveryEnvelope, WebhookSink webhookSink, CancellationToken cancellationToken = default)
            => Task.FromResult(new DispatchHandoffResult(Name, DispatchHandoffStatus.Enqueued, deliveryEnvelope.EventId));
    }
}
