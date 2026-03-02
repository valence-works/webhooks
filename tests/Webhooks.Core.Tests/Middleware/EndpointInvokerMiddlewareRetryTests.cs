using System.Net;
using System.Net.Http;
using Webhooks.Core.Options;
using Webhooks.Core.Services;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Middleware;

public class EndpointInvokerMiddlewareRetryTests
{
    [Fact]
    public async Task InvokeAsync_Executes_Endpoint_Middleware_Per_Retry_Attempt()
    {
        var handler = new SequenceHandler(HttpStatusCode.InternalServerError, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var middleware = new CountingEndpointMiddleware();
        var invoker = new HttpWebhookEndpointInvoker(
            httpClient,
            new StaticClock(DateTimeOffset.UtcNow),
            new IWebhookEndpointInvokerMiddleware[] { middleware },
            new DefaultTransientFailureDetectionStrategy(),
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions { RetryAttempts = 2 }));

        var sink = new WebhookSink
        {
            SinkId = "sink-a",
            Destination = new Uri("https://example.com/a"),
            Subscriptions = new List<WebhookEventFilter> { new() { EventType = "order.created" } }
        };

        await invoker.InvokeAsync(sink, new NewWebhookEvent("order.created", new { id = "1" }, EventId: "evt-1"));

        Assert.Equal(2, middleware.InvocationCount);
    }

    private sealed class CountingEndpointMiddleware : IWebhookEndpointInvokerMiddleware
    {
        public int InvocationCount { get; private set; }

        public async Task<DeliveryResult> InvokeAsync(WebhookEndpointInvocationContext context, Func<WebhookEndpointInvocationContext, CancellationToken, Task<DeliveryResult>> next, CancellationToken cancellationToken = default)
        {
            InvocationCount++;
            return await next(context, cancellationToken);
        }
    }

    private sealed class SequenceHandler(params HttpStatusCode[] sequence) : HttpMessageHandler
    {
        private int _index;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var current = _index < sequence.Length ? sequence[_index] : sequence[^1];
            _index++;
            return Task.FromResult(new HttpResponseMessage(current));
        }
    }

    private sealed class StaticClock(DateTimeOffset value) : ISystemClock
    {
        public DateTimeOffset UtcNow => value;
    }
}
