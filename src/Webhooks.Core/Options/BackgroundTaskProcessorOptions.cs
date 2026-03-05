namespace Webhooks.Core.Options;

/// <summary>
/// Configuration options for the background task processor channel and workers.
/// </summary>
public sealed class BackgroundTaskProcessorOptions
{
    /// <summary>
    /// The maximum number of tasks the background processor channel can hold.
    /// </summary>
    public int ChannelCapacity { get; set; } = 1000;
    
    /// <summary>
    /// The maximum number of outbound HTTP requests to send in parallel.
    /// </summary>
    public int MaxDegreeOfParallelism { get; set; } = 5;
}