using Microsoft.Extensions.Logging.Abstractions;
using Webhooks.Core.Options;
using Webhooks.Core.Services;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Middleware;

public class BroadcastMiddlewareOrderingTests
{
    [Fact]
    public async Task BroadcastAsync_Executes_Middleware_In_Configured_Order()
    {
        var order = new List<string>();
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "order.created" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var middlewares = new IBroadcastMiddleware[]
        {
            new RecordingBroadcastMiddleware("m1", order),
            new RecordingBroadcastMiddleware("m2", order)
        };

        var broadcaster = new DefaultWebhookEventBroadcaster(
            sinkProvider,
            coordinator,
            new StaticClock(new DateTimeOffset(2026, 03, 01, 12, 0, 0, TimeSpan.Zero)),
            new SequentialBroadcasterStrategy(),
            middlewares,
            new[] { new JsonPathPayloadFieldSelectorStrategy() },
            new[] { new ScalarStringEqualityComparisonStrategy() },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions()),
            NullLogger<DefaultWebhookEventBroadcaster>.Instance);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("order.created", new { id = "1" }));

        Assert.Equal(new[] { "m1-before", "m2-before", "m2-after", "m1-after" }, order);
    }

    private sealed class RecordingBroadcastMiddleware(string name, List<string> order) : IBroadcastMiddleware
    {
        public async Task InvokeAsync(DeliveryEnvelope deliveryEnvelope, Func<DeliveryEnvelope, CancellationToken, Task> next, CancellationToken cancellationToken = default)
        {
            order.Add($"{name}-before");
            await next(deliveryEnvelope, cancellationToken);
            order.Add($"{name}-after");
        }
    }

    private sealed class TestWebhookSinkProvider(IEnumerable<WebhookSink> sinks) : IWebhookSinkProvider
    {
        public ValueTask<IEnumerable<WebhookSink>> ListAsync(CancellationToken cancellationToken = default)
            => ValueTask.FromResult(sinks);
    }

    private sealed class RecordingCoordinator : IDispatcherInvocationCoordinator
    {
        public Task<DispatchHandoffResult> DispatchAsync(DeliveryEnvelope deliveryEnvelope, WebhookSink webhookSink, CancellationToken cancellationToken = default)
            => Task.FromResult(new DispatchHandoffResult("default", DispatchHandoffStatus.Accepted, deliveryEnvelope.EventId));
    }

    private sealed class StaticClock(DateTimeOffset value) : ISystemClock
    {
        public DateTimeOffset UtcNow => value;
    }
}
