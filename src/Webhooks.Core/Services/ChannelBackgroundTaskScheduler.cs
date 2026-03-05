namespace Webhooks.Core.Services;

/// <summary>
/// Schedules work items by writing them to a background task channel.
/// </summary>
public sealed class ChannelBackgroundTaskScheduler(IBackgroundTaskChannel channel) : IBackgroundTaskScheduler
{
    public async Task EnqueueWorkAsync(Func<Task> work, CancellationToken cancellationToken = default)
    {
        await channel.Writer.WriteAsync(work, cancellationToken);
    }
}