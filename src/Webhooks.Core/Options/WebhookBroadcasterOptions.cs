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

        if (options.DefaultDispatcher is { Length: 0 })
            return ValidateOptionsResult.Fail("DefaultDispatcher cannot be empty.");

        if (options.RetryAttempts <= 0)
            return ValidateOptionsResult.Fail("RetryAttempts must be greater than zero.");
        
        return ValidateOptionsResult.Success;
    }
}