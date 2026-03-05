using Microsoft.Extensions.Hosting;

namespace Webhooks.Core.HostedServices;

/// <summary>
/// Hosted service that starts and stops the background task processor.
/// </summary>
public sealed class StartBackgroundProcessor(IBackgroundTaskProcessor backgroundTaskProcessor, IBackgroundTaskChannel backgroundTaskChannel) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await backgroundTaskProcessor.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        backgroundTaskChannel.Writer.Complete();
        await backgroundTaskProcessor.StopAsync(cancellationToken);
    }
}