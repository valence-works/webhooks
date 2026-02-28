using Microsoft.Extensions.Options;

namespace Webhooks.Core.Options;

public class BackgroundTaskProcessorOptions
{
    /// The maximum number of tasks the background processor channel can hold.
    public int ChannelCapacity { get; set; } = 1000;
    
    /// The maximum number of outbound HTTP requests to send in parallel. 
    public int MaxDegreeOfParallelism { get; set; } = 5;
}

public class ConfigureBackgroundTaskProcessorOptions(Action<BackgroundTaskProcessorOptions>? action) : ConfigureOptions<BackgroundTaskProcessorOptions>(action)
{
}