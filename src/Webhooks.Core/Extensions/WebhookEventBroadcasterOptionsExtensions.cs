using Webhooks.Core.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core;

public static class WebhookEventBroadcasterOptionsExtensions
{
    public static WebhookBroadcasterOptions UseStrategy<T>(this WebhookBroadcasterOptions options) where T : IBroadcasterStrategy
    {
        options.BroadcasterStrategy = typeof(T);
        return options;
    }
    
    public static WebhookBroadcasterOptions UseStrategy(this WebhookBroadcasterOptions options, Type strategyType)
    {
        if(!strategyType.IsAssignableTo(typeof(IBroadcasterStrategy)))
            throw new ArgumentException($"{nameof(strategyType)} must be assignable from {nameof(IBroadcasterStrategy)}");
        
        options.BroadcasterStrategy = strategyType;
        return options;
    }

    public static WebhookBroadcasterOptions UseSequentialBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<SequentialBroadcasterStrategy>();
    public static WebhookBroadcasterOptions UseParallelTaskBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<ParallelTaskBroadcasterStrategy>();
    public static WebhookBroadcasterOptions UseBackgroundProcessorBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<BackgroundProcessorBroadcasterStrategy>();
}