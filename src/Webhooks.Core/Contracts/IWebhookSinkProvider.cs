namespace Webhooks.Core;

/// <summary>
/// Provides a list of all registered webhook sinks that are interested in a given event.
/// </summary>
public interface IWebhookSinkProvider
{
    /// <summary>
    /// Returns all registered webhook sinks.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    ValueTask<IEnumerable<WebhookSink>> ListAsync(CancellationToken cancellationToken = default);
}