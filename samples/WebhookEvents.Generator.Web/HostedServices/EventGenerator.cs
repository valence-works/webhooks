using Webhooks.Core;

namespace WebhookEvents.Generator.Web.HostedServices;

// A simple background task that periodically generates heartbeat events that get sent to webhook endpoints.
public class EventGenerator(IWebhookEventBroadcaster webhookEventBroadcaster, TimeProvider timeProvider) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    // ReSharper disable once NotAccessedField.Local
    // Keep this to ensure the Timer object doesn't get collected by the GC.
    private Timer _timer = default!;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(OnTimerTick, null, _interval, _interval);
        return Task.CompletedTask;
    }

    private async void OnTimerTick(object? state)
    {
        var now = timeProvider.GetUtcNow();
        var payload = new Heartbeat(now);
        await webhookEventBroadcaster.BroadcastAsync("Heartbeat", payload);
    }
}