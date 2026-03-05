namespace Webhooks.Core.Strategies;

/// <summary>
/// Broadcasts to matching sinks one at a time in sequence.
/// </summary>
public sealed class SequentialBroadcasterStrategy : IBroadcasterStrategy
{
    public async Task BroadcastAsync(IEnumerable<WebhookSink> webhookEndpoints, Func<WebhookSink, Task> invocation, CancellationToken cancellationToken = default)
    {
        foreach (var webhookEndpoint in webhookEndpoints) await invocation(webhookEndpoint);
    }
}