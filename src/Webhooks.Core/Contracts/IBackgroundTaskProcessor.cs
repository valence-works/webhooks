namespace Webhooks.Core;

/// <summary>
/// Processes background work items dequeued from a channel.
/// </summary>
public interface IBackgroundTaskProcessor
{
    /// <summary>
    /// Starts the background processor workers.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the start operation.</param>
    ValueTask StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops all background processor workers and waits for completion.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the stop operation.</param>
    ValueTask StopAsync(CancellationToken cancellationToken = default);
}