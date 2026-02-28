using Microsoft.Extensions.Hosting;

namespace Webhooks.Core.HostedServices;

public class StartBackgroundProcessor(IBackgroundTaskProcessor backgroundTaskProcessor, IBackgroundTaskChannel backgroundTaskChannel) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await backgroundTaskProcessor.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        backgroundTaskChannel.Writer.Complete();
        await backgroundTaskProcessor.StopAsync();
    }
}