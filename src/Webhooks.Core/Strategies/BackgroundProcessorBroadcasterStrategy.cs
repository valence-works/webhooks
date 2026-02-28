namespace Webhooks.Core.Strategies;

public class BackgroundProcessorBroadcasterStrategy(IBackgroundTaskScheduler scheduler) : IBroadcasterStrategy
{
    public async Task BroadcastAsync(IEnumerable<WebhookSink> webhookEndpoints, Func<WebhookSink, Task> invocation, CancellationToken cancellationToken = default)
    {
        foreach (var webhookEndpoint in webhookEndpoints)
            await scheduler.EnqueueWork(async () => await invocation(webhookEndpoint), cancellationToken);
    }
}