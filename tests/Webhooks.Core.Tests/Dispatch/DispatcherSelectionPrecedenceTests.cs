using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Webhooks.Core.Options;
using Webhooks.Core.Services;

namespace Webhooks.Core.Tests.Dispatch;

public class DispatcherSelectionPrecedenceTests
{
    [Fact]
    public async Task DispatchAsync_Uses_Sink_Dispatcher_Override_Before_Default()
    {
        var defaultDispatcher = new RecordingDispatcher("default");
        var customDispatcher = new RecordingDispatcher("custom");
        var coordinator = new DispatcherInvocationCoordinator(
            new IWebhookDispatcher[] { defaultDispatcher, customDispatcher },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions { DefaultDispatcher = "default" }),
            NullLogger<DispatcherInvocationCoordinator>.Instance);

        var envelope = new DeliveryEnvelope("evt-1", "order.created", null, DateTimeOffset.UtcNow);
        var sink = new WebhookSink
        {
            SinkId = "sink-a",
            Destination = new Uri("https://example.com/a"),
            Dispatcher = "custom"
        };

        await coordinator.DispatchAsync(envelope, sink);

        Assert.Equal(0, defaultDispatcher.CallCount);
        Assert.Equal(1, customDispatcher.CallCount);
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
