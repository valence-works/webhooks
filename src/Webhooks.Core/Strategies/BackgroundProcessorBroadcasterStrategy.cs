namespace Webhooks.Core.Strategies;

/// <summary>
/// Broadcasts by enqueuing each sink invocation onto the background task scheduler.
/// </summary>
public sealed class BackgroundProcessorBroadcasterStrategy(IBackgroundTaskScheduler scheduler) : IBroadcasterStrategy
{
    public async Task BroadcastAsync(IEnumerable<WebhookSink> webhookEndpoints, Func<WebhookSink, Task> invocation, CancellationToken cancellationToken = default)
    {
        foreach (var webhookEndpoint in webhookEndpoints)
            await scheduler.EnqueueWorkAsync(async () => await invocation(webhookEndpoint), cancellationToken);
    }
}