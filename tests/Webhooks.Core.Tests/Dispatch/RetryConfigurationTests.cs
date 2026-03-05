using System.Net;
using Webhooks.Core.Options;
using Webhooks.Core.Services;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Dispatch;

public sealed class RetryConfigurationTests
{
    [Fact]
    public async Task InvokeAsync_Uses_Configured_Retry_Attempts()
    {
        var handler = new CountingStatusHandler(HttpStatusCode.InternalServerError);
        var invoker = new HttpWebhookEndpointInvoker(
            new HttpClient(handler),
            new StaticClock(DateTimeOffset.UtcNow),
            Array.Empty<IWebhookEndpointInvokerMiddleware>(),
            new DefaultTransientFailureDetectionStrategy(),
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions { RetryAttempts = 1 }));

        var sink = new WebhookSink
        {
            SinkId = "sink-a",
            Destination = new Uri("https://example.com/a"),
            Subscriptions = new List<WebhookEventFilter> { new() { EventType = "order.created" } }
        };

        await Assert.ThrowsAsync<HttpRequestException>(() => invoker.InvokeAsync(sink, new NewWebhookEvent("order.created")));
        Assert.Equal(1, handler.CallCount);
    }

    private sealed class CountingStatusHandler(HttpStatusCode statusCode) : HttpMessageHandler
    {
        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(new HttpResponseMessage(statusCode));
        }
    }

    private sealed class StaticClock(DateTimeOffset value) : ISystemClock
    {
        public DateTimeOffset UtcNow => value;
    }
}
