namespace Webhooks.Core.Strategies;

public class ParallelTaskBroadcasterStrategy : IBroadcasterStrategy
{
    public async Task BroadcastAsync(IEnumerable<WebhookSink> webhookEndpoints, Func<WebhookSink, Task> invocation, CancellationToken cancellationToken = default)
    {
        await Task.WhenAll(webhookEndpoints.Select(invocation));
    }
}