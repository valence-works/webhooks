using Microsoft.Extensions.Logging;

namespace Webhooks.Core.Services;

public class ChannelBackgroundTaskProcessor(IBackgroundTaskChannel channel, ILogger<ChannelBackgroundTaskProcessor> logger) : IBackgroundTaskProcessor
{
    private const int InitialWorkerCount = 5;
    private readonly List<Task> _tasks = new();
    private bool _isStarted;
    private CancellationTokenSource _cts = default!;

    public ValueTask StartAsync()
    {
        _cts = new CancellationTokenSource();
        _isStarted = true;

        for (var i = 0; i < InitialWorkerCount; i++)
            SpawnWorker();

        return default;
    }

    public async ValueTask StopAsync()
    {
        if (!_isStarted)
            return;

        _cts.Cancel();
        await Task.WhenAll(_tasks); // Wait for all tasks to finish.
        _tasks.Clear();

        _isStarted = false;
    }

    public async Task Wait() => await Task.WhenAll(_tasks);

    private void SpawnWorker()
    {
        _tasks.Add(Task.Run(async () =>
        {
            try
            {
                await foreach (var workItem in channel.Reader.ReadAllAsync(_cts.Token).ConfigureAwait(false))
                {
                    await workItem();
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Worker stopped");
            }
        }));
    }
}