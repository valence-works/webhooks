using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Webhooks.Core.Options;

namespace Webhooks.Core.HostedServices;

/// <summary>
/// Hosted service that eagerly validates options on application startup.
/// </summary>
public sealed class ValidateOptionsOnStart(
    IOptions<WebhookSinksOptions> sinksOptions,
    IOptions<WebhookBroadcasterOptions> broadcasterOptions) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Accessing .Value triggers IValidateOptions validation.
        _ = sinksOptions.Value;
        _ = broadcasterOptions.Value;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
