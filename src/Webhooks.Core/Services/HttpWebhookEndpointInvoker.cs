using System.Net.Http.Json;
using Polly;
using Polly.Extensions.Http;
using WebhooksCore;

namespace Webhooks.Core.Services;

public class HttpWebhookEndpointInvoker(HttpClient httpClient, ISystemClock systemClock) : IWebhookEndpointInvoker
{
    public async Task InvokeAsync(WebhookSink webhookSink, NewWebhookEvent newWebhookEvent, CancellationToken cancellationToken = default)
    {
        var retryPolicy = GetRetryPolicy();
        var webhookEvent = new WebhookEvent(newWebhookEvent.EventType, newWebhookEvent.Payload, systemClock.UtcNow);

        var response = await retryPolicy.ExecuteAsync(
            async () => await httpClient.PostAsJsonAsync(webhookSink.Url, webhookEvent, cancellationToken));

        response.EnsureSuccessStatusCode();
    }

    private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        // TODO: Make retry policies configurable.
        var retryAttempts = 3;

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .RetryAsync(retryAttempts);
    }
}