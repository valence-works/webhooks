using Microsoft.Extensions.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Options;

public class WebhookBroadcasterOptions
{
    public Type BroadcasterStrategy { get; set; } = typeof(SequentialBroadcasterStrategy);
    public string? DefaultDispatcher { get; set; }
    public int? QueueCapacity { get; set; }
    public int? WorkerParallelism { get; set; }
    public OverflowPolicy OverflowPolicy { get; set; } = OverflowPolicy.FailFast;
    public bool DeduplicationEnabled { get; set; }
    public int RetryAttempts { get; set; } = 3;

    /// <summary>
    /// Maximum number of event IDs to retain in the in-memory deduplication store.
    /// When the limit is reached, the oldest entry is evicted (FIFO) to make room for new ones.
    /// Set to 0 to disable eviction (unbounded store). Defaults to 1,000.
    /// </summary>
    public int MaxDeduplicationEntries { get; set; } = 1_000;
}

public enum OverflowPolicy
{
    FailFast,
    DropOldest,
    Block
}

public class ConfigureWebhookEventBroadcasterOptions : IValidateOptions<WebhookBroadcasterOptions>
{
    public ValidateOptionsResult Validate(string? name, WebhookBroadcasterOptions options)
    {
        if (!options.BroadcasterStrategy.IsAssignableTo(typeof(IBroadcasterStrategy)))
            return ValidateOptionsResult.Fail($"BroadcasterStrategy type is not assignable to IBroadcasterStrategy");

        if (options.QueueCapacity is <= 0)
            return ValidateOptionsResult.Fail("QueueCapacity must be greater than zero when configured.");

        if (options.WorkerParallelism is <= 0)
            return ValidateOptionsResult.Fail("WorkerParallelism must be greater than zero when configured.");

        if (options.DefaultDispatcher is not null && string.IsNullOrWhiteSpace(options.DefaultDispatcher))
            return ValidateOptionsResult.Fail("DefaultDispatcher cannot be empty or whitespace.");

        if (options.RetryAttempts <= 0)
            return ValidateOptionsResult.Fail("RetryAttempts must be greater than zero.");
        
        return ValidateOptionsResult.Success;
    }
}