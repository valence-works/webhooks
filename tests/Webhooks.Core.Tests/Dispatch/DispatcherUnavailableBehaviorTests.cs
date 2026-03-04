using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Webhooks.Core.Options;
using Webhooks.Core.Services;

namespace Webhooks.Core.Tests.Dispatch;

public class DispatcherUnavailableBehaviorTests
{
    [Fact]
    public async Task DispatchAsync_Returns_Rejected_When_No_Dispatcher_Is_Available()
    {
        var coordinator = new DispatcherInvocationCoordinator(
            Array.Empty<IWebhookDispatcher>(),
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions()),
            NullLogger<DispatcherInvocationCoordinator>.Instance);

        var result = await coordinator.DispatchAsync(
            new DeliveryEnvelope("evt-1", "order.created", null, DateTimeOffset.UtcNow),
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a")
            });

        Assert.Equal(DispatchHandoffStatus.Rejected, result.HandoffStatus);
    }
}
