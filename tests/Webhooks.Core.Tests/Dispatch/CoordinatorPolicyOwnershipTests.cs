using Microsoft.Extensions.Logging.Abstractions;
using Webhooks.Core.Options;
using Webhooks.Core.Services;

namespace Webhooks.Core.Tests.Dispatch;

public sealed class CoordinatorPolicyOwnershipTests
{
    [Fact]
    public async Task DispatchAsync_Invokes_Exactly_One_Dispatcher_Per_Attempt()
    {
        var dispatcherA = new RecordingDispatcher("a");
        var dispatcherB = new RecordingDispatcher("b");
        var coordinator = new DispatcherInvocationCoordinator(
            new IWebhookDispatcher[] { dispatcherA, dispatcherB },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions { DefaultDispatcher = "a" }),
            NullLogger<DispatcherInvocationCoordinator>.Instance);

        await coordinator.DispatchAsync(
            new DeliveryEnvelope("evt-1", "order.created", null, DateTimeOffset.UtcNow),
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a")
            });

        Assert.Equal(1, dispatcherA.CallCount);
        Assert.Equal(0, dispatcherB.CallCount);
    }

    private sealed class RecordingDispatcher(string name) : IWebhookDispatcher
    {
        public int CallCount { get; private set; }
        public string Name => name;

        public Task<DispatchHandoffResult> DispatchAsync(DeliveryEnvelope deliveryEnvelope, WebhookSink webhookSink, CancellationToken cancellationToken = default)
        {
            CallCount++;
            return Task.FromResult(new DispatchHandoffResult(Name, DispatchHandoffStatus.Accepted, deliveryEnvelope.EventId));
        }
    }
}
