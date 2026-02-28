namespace Webhooks.Core;

/// Provides a list of all registered webhook sinks that are interested in a given event.
public interface IWebhookSinkProvider
{
    ValueTask<IEnumerable<WebhookSink>> ListAsync(CancellationToken cancellationToken = default);
}