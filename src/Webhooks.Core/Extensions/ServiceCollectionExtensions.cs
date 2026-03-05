using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Webhooks.Core.HostedServices;
using Webhooks.Core.Options;
using Webhooks.Core.Serialization.Converters;
using Webhooks.Core.Services;
using Webhooks.Core.SinkProviders;
using Webhooks.Core.Strategies;

namespace Webhooks.Core;

/// <summary>
/// Extension methods for registering webhook core services with dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all webhook core services, options, and the default broadcaster.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureHttpClient">Optional callback to configure the outbound HTTP client.</param>
    public static IServiceCollection AddWebhooksCore(this IServiceCollection services, Action<IHttpClientBuilder>? configureHttpClient = null)
    {
        TypeDescriptor.AddAttributes(typeof(Type), new TypeConverterAttribute(typeof(TypeTypeConverter)));
        
        services.AddOptions<WebhookSinksOptions>();
        services.AddOptions<BackgroundTaskProcessorOptions>();
        services.AddOptions<WebhookBroadcasterOptions>();
        services.AddSingleton<IValidateOptions<WebhookBroadcasterOptions>, ConfigureWebhookEventBroadcasterOptions>();
        services.AddSingleton<IValidateOptions<WebhookSinksOptions>, ValidateWebhookSinksOptions>();

        var httpClientBuilder = services.AddHttpClient<HttpWebhookEndpointInvoker>();
        configureHttpClient?.Invoke(httpClientBuilder);
        
        return services.AddSingleton<IWebhookEventBroadcaster, DefaultWebhookEventBroadcaster>()
            .AddSingleton<IWebhookSinkProvider, OptionsWebhookSinkProvider>()
            .AddSingleton<IWebhookEndpointInvoker>(sp => sp.GetRequiredService<HttpWebhookEndpointInvoker>())
            .AddSingleton<IDispatcherInvocationCoordinator, DispatcherInvocationCoordinator>()
            .AddSingleton<IWebhookDispatcher, DefaultWebhookDispatcher>()
            .AddSingleton<IPayloadFieldSelectorStrategy, JsonPathPayloadFieldSelectorStrategy>()
            .AddSingleton<IPayloadValueComparisonStrategy, ScalarStringEqualityComparisonStrategy>()
            .AddSingleton<ITransientFailureDetectionStrategy, DefaultTransientFailureDetectionStrategy>()
            .AddSingleton<IBackgroundTaskProcessor, ChannelBackgroundTaskProcessor>()
            .AddSingleton<IBackgroundTaskScheduler, ChannelBackgroundTaskScheduler>()
            .AddSingleton<IBackgroundTaskChannel, BackgroundTaskChannel>()
            .AddSingleton<ISystemClock, DefaultSystemClock>()
            .AddSingleton(CreateBroadcasterStrategy)
            .AddHostedService<ValidateOptionsOnStart>();
    }

    /// <summary>
    /// Registers the background processor hosted service for deferred webhook delivery.
    /// </summary>
    public static IServiceCollection AddWebhooksBackgroundProcessor(this IServiceCollection services)
    {
        return services.AddHostedService<StartBackgroundProcessor>();
    }

    private static IBroadcasterStrategy CreateBroadcasterStrategy(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<WebhookBroadcasterOptions>>();
        var type = options.Value.BroadcasterStrategy;
        var broadcasterStrategy = (IBroadcasterStrategy)ActivatorUtilities.CreateInstance(serviceProvider, type);
        return broadcasterStrategy;
    }
}