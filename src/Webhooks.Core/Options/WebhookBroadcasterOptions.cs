using Webhooks.Core.Strategies;

namespace Webhooks.Core.Options;

/// <summary>
/// Configuration options for the webhook event broadcaster.
/// </summary>
public sealed class WebhookBroadcasterOptions
{
    /// <summary>
    /// Gets or sets the broadcaster strategy type. Defaults to <see cref="SequentialBroadcasterStrategy"/>.
    /// </summary>
    public Type BroadcasterStrategy { get; set; } = typeof(SequentialBroadcasterStrategy);

    /// <summary>
    /// Gets or sets the default dispatcher name. When <c>null</c>, resolved by convention.
    /// </summary>
    public string? DefaultDispatcher { get; set; }

    /// <summary>
    /// Gets or sets the maximum queue capacity for background processing.
    /// </summary>
    public int? QueueCapacity { get; set; }

    /// <summary>
    /// Gets or sets the number of parallel background worker tasks.
    /// </summary>
    public int? WorkerParallelism { get; set; }

    /// <summary>
    /// Gets or sets the policy applied when the background queue is full.
    /// </summary>
    public OverflowPolicy OverflowPolicy { get; set; } = OverflowPolicy.FailFast;

    /// <summary>
    /// Gets or sets whether event deduplication is enabled.
    /// </summary>
    public bool DeduplicationEnabled { get; set; }

    /// <summary>
    /// Gets or sets the number of retry attempts for failed deliveries. Defaults to 3.
    /// </summary>
    public int RetryAttempts { get; set; } = 3;

    /// <summary>
    /// Maximum number of event IDs to retain in the in-memory deduplication store.
    /// When the limit is reached, the oldest entry is evicted (FIFO) to make room for new ones.
    /// Set to 0 to disable eviction (unbounded store). Defaults to 1,000.
    /// </summary>
    public int MaxDeduplicationEntries { get; set; } = 1_000;
}