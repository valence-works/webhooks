using Microsoft.Extensions.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Options;

public class WebhookBroadcasterOptions
{
    public Type BroadcasterStrategy { get; set; } = typeof(SequentialBroadcasterStrategy);
}

public class ConfigureWebhookEventBroadcasterOptions : IValidateOptions<WebhookBroadcasterOptions>
{
    public ValidateOptionsResult Validate(string? name, WebhookBroadcasterOptions options)
    {
        if(!options.BroadcasterStrategy.IsAssignableTo(typeof(IBroadcasterStrategy)))
            return ValidateOptionsResult.Fail($"BroadcasterStrategy type is not assignable to IBroadcasterStrategy");
        
        return ValidateOptionsResult.Success;
    }
}