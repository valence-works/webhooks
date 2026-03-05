using Webhooks.Core;

namespace WebhookEvents.Generator.Web.HostedServices;

/// <summary>
/// A simple background task that periodically generates heartbeat events that get sent to webhook endpoints.
/// </summary>
public sealed class EventGenerator(IWebhookEventBroadcaster webhookEventBroadcaster, TimeProvider timeProvider) : BackgroundService
{
    private readonly TimeSpan interval = TimeSpan.FromSeconds(5);

    // ReSharper disable once NotAccessedField.Local
    // Keep this to ensure the Timer object doesn't get collected by the GC.
    private Timer timer = default!;
    private CancellationToken stoppingToken;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.stoppingToken = stoppingToken;
        timer = new Timer(OnTimerTick, null, interval, interval);
        return Task.CompletedTask;
    }

    private async void OnTimerTick(object? state)
    {
        if (stoppingToken.IsCancellationRequested)
            return;

        var now = timeProvider.GetUtcNow();
        var payload = new Heartbeat(now);
        await webhookEventBroadcaster.BroadcastAsync("Heartbeat", payload, stoppingToken);
    }
}