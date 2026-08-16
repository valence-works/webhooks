using WebhookEvents;
using Webhooks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/webhooks", (WebhookEvent webhookEvent) =>
{
    Console.WriteLine($"{webhookEvent.EventType} event received at: {webhookEvent.Timestamp}");
});

app.MapPost("/webhooks/heartbeat", (WebhookEvent<Heartbeat> webhookEvent) =>
{
    var heartbeat = webhookEvent.Payload!;
    Console.WriteLine($"Heartbeat at {heartbeat.Timestamp}");
});

app.Run();