namespace Webhooks.Core.Services;

public class ChannelBackgroundTaskScheduler(IBackgroundTaskChannel channel) : IBackgroundTaskScheduler
{
    public async Task EnqueueWork(Func<Task> work, CancellationToken cancellationToken = default)
    {
        await channel.Writer.WriteAsync(work, cancellationToken);
    }
}