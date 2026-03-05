using System.Threading.Channels;
using Microsoft.Extensions.Options;
using Webhooks.Core.Options;

namespace Webhooks.Core.Services;

/// <summary>
/// A bounded channel for queuing background work items.
/// </summary>
public sealed class BackgroundTaskChannel(IOptions<BackgroundTaskProcessorOptions> options) : IBackgroundTaskChannel
{
    /// <summary>
    /// Gets the underlying channel instance.
    /// </summary>
    public Channel<Func<Task>> Channel { get; } = System.Threading.Channels.Channel.CreateBounded<Func<Task>>(new BoundedChannelOptions(options.Value.ChannelCapacity)
    {
        FullMode = BoundedChannelFullMode.Wait
    });

    public ChannelWriter<Func<Task>> Writer => Channel.Writer;
    public ChannelReader<Func<Task>> Reader => Channel.Reader;
}