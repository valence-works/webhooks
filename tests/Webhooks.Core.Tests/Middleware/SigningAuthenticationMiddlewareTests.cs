using System.Net;
using System.Net.Http;
using Webhooks.Core.Options;
using Webhooks.Core.Services;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Middleware;

public class SigningAuthenticationMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_Uses_Custom_Endpoint_Middleware_Extension_Point()
    {
        var middleware = new SigningMiddleware();
        var invoker = new HttpWebhookEndpointInvoker(
            new HttpClient(new OkHandler()),
            new StaticClock(DateTimeOffset.UtcNow),
            new IWebhookEndpointInvokerMiddleware[] { middleware },
            new DefaultTransientFailureDetectionStrategy(),
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions { RetryAttempts = 1 }));

        var sink = new WebhookSink
        {
            SinkId = "sink-a",
            Destination = new Uri("https://example.com/a"),
            Subscriptions = new List<WebhookEventFilter> { new() { EventType = "order.created" } }
        };

        await invoker.InvokeAsync(sink, new NewWebhookEvent("order.created", new { id = "1" }));

        Assert.True(middleware.WasInvoked);
    }

    private sealed class SigningMiddleware : IWebhookEndpointInvokerMiddleware
    {
        public bool WasInvoked { get; private set; }

        public async Task<DeliveryResult> InvokeAsync(WebhookEndpointInvocationContext context, Func<WebhookEndpointInvocationContext, CancellationToken, Task<DeliveryResult>> next, CancellationToken cancellationToken = default)
        {
            WasInvoked = true;
            return await next(context, cancellationToken);
        }
    }

    private sealed class OkHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
    }

    private sealed class StaticClock(DateTimeOffset value) : ISystemClock
    {
        public DateTimeOffset UtcNow => value;
    }
}
