namespace Webhooks.Core;

public interface IBackgroundTaskProcessor
{
    ValueTask StartAsync();
    ValueTask StopAsync();
}