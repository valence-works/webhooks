namespace Webhooks.Core.Strategies;

public class SequentialBroadcasterStrategy : IBroadcasterStrategy
{
    public async Task BroadcastAsync(IEnumerable<WebhookSink> webhookEndpoints, Func<WebhookSink, Task> invocation, CancellationToken cancellationToken = default)
    {
        foreach (var webhookEndpoint in webhookEndpoints) await invocation(webhookEndpoint);
    }
}