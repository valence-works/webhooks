using System.Threading.Channels;

namespace Webhooks.Core;

/// <summary>
/// Provides a channel for queuing and reading background work items.
/// </summary>
public interface IBackgroundTaskChannel
{
    /// <summary>
    /// Gets the writer used to enqueue work items.
    /// </summary>
    ChannelWriter<Func<Task>> Writer { get; }

    /// <summary>
    /// Gets the reader used to dequeue work items.
    /// </summary>
    ChannelReader<Func<Task>> Reader { get; }
}