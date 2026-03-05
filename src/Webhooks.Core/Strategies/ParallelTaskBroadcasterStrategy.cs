namespace Webhooks.Core.Strategies;

/// <summary>
/// Broadcasts to all matching sinks concurrently using parallel task execution.
/// </summary>
public sealed class ParallelTaskBroadcasterStrategy : IBroadcasterStrategy
{
    public async Task BroadcastAsync(IEnumerable<WebhookSink> webhookEndpoints, Func<WebhookSink, Task> invocation, CancellationToken cancellationToken = default)
    {
        var tasks = webhookEndpoints.Select(invocation).ToList();
        await Task.WhenAll(tasks).WaitAsync(cancellationToken);
    }
}