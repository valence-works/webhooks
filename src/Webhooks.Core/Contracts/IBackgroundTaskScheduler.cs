namespace Webhooks.Core;

/// <summary>
/// Schedules background work items for deferred execution.
/// </summary>
public interface IBackgroundTaskScheduler
{
    /// <summary>
    /// Enqueues a work item for background processing.
    /// </summary>
    /// <param name="work">The work item to execute.</param>
    /// <param name="cancellationToken">A token to cancel the enqueue operation.</param>
    Task EnqueueWorkAsync(Func<Task> work, CancellationToken cancellationToken = default);
}