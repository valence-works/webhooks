using Microsoft.Extensions.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Options;

/// <summary>
/// Validates <see cref="WebhookBroadcasterOptions"/> on startup.
/// </summary>
public sealed class ConfigureWebhookEventBroadcasterOptions : IValidateOptions<WebhookBroadcasterOptions>
{
    public ValidateOptionsResult Validate(string? name, WebhookBroadcasterOptions options)
    {
        if (!options.BroadcasterStrategy.IsAssignableTo(typeof(IBroadcasterStrategy)))
            return ValidateOptionsResult.Fail($"BroadcasterStrategy type is not assignable to IBroadcasterStrategy");

        if (!options.EventTypeMatcherStrategy.IsAssignableTo(typeof(IEventTypeMatcherStrategy)))
            return ValidateOptionsResult.Fail($"EventTypeMatcherStrategy type is not assignable to IEventTypeMatcherStrategy");

        if (options.QueueCapacity is <= 0)
            return ValidateOptionsResult.Fail("QueueCapacity must be greater than zero when configured.");

        if (options.WorkerParallelism is <= 0)
            return ValidateOptionsResult.Fail("WorkerParallelism must be greater than zero when configured.");

        if (options.DefaultDispatcher is not null && string.IsNullOrWhiteSpace(options.DefaultDispatcher))
            return ValidateOptionsResult.Fail("DefaultDispatcher cannot be empty or whitespace.");

        if (options.RetryAttempts <= 0)
            return ValidateOptionsResult.Fail("RetryAttempts must be greater than zero.");

        if (options.MaxDeduplicationEntries < 0)
            return ValidateOptionsResult.Fail("MaxDeduplicationEntries must not be negative.");
        
        return ValidateOptionsResult.Success;
    }
}
