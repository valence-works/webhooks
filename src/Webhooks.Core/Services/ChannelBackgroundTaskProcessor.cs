using Microsoft.Extensions.Logging;

namespace Webhooks.Core.Services;

/// <summary>
/// Processes background tasks by spawning workers that read from a channel.
/// </summary>
public sealed class ChannelBackgroundTaskProcessor(IBackgroundTaskChannel channel, ILogger<ChannelBackgroundTaskProcessor> logger) : IBackgroundTaskProcessor
{
    private const int InitialWorkerCount = 5;
    private readonly List<Task> tasks = new();
    private bool isStarted;
    private CancellationTokenSource cts = default!;

    public ValueTask StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        cts = new CancellationTokenSource();
        isStarted = true;

        for (var i = 0; i < InitialWorkerCount; i++)
            SpawnWorker();

        return default;
    }

    public async ValueTask StopAsync(CancellationToken cancellationToken = default)
    {
        if (!isStarted)
            return;

#if NET8_0_OR_GREATER
        await cts.CancelAsync().ConfigureAwait(false);
#else
        cts.Cancel();
        await Task.CompletedTask; // Ensure async signature is satisfied.
#endif
        await Task.WhenAll(tasks).WaitAsync(cancellationToken); // Wait for all tasks to finish, respecting the shutdown deadline.
        tasks.Clear();

        isStarted = false;
    }

    /// <summary>
    /// Waits for all spawned worker tasks to complete.
    /// </summary>
    public async Task WaitAsync() => await Task.WhenAll(tasks);

    private void SpawnWorker()
    {
        tasks.Add(Task.Run(async () =>
        {
            try
            {
                await foreach (var workItem in channel.Reader.ReadAllAsync(cts.Token).ConfigureAwait(false))
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