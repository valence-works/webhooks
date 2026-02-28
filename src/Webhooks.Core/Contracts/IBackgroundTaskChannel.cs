using System.Threading.Channels;

namespace Webhooks.Core;

public interface IBackgroundTaskChannel
{
    ChannelWriter<Func<Task>> Writer { get; }
    ChannelReader<Func<Task>> Reader { get; }
}