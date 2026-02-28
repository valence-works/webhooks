namespace Webhooks.Core;

/// Provides a list of all registered webhook sources.
public interface IWebhookSourceProvider
{
    ValueTask<IEnumerable<WebhookSource>> ListAsync(CancellationToken cancellationToken = default);
}