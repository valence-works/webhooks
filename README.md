# Webhooks Core
Webhooks Core is a .NET library that allows your applications to easily broadcast events via Webhooks. This is especially useful when you need to inform external applications of specific events occurring in your system.

## Features

* Register external applications to receive webhook events.
* Broadcast any custom event to registered applications.
* Configure multiple webhook endpoints called _sinks_.

## Getting Started

Here a simple JSON configuration for registering webhook endpoints:

```json
{
  "Webhooks": {
    "Sinks": [
      {
        "Id": "ExternalApplication1",
        "Name": "External Application 1",
        "EventTypes": [
          "*"
        ],
        "Url": "https://localhost:6001/webhooks"
      },
      {
        "Id": "ExternalApplication2",
        "Name": "External Application 2",
        "EventTypes": [
          "Heartbeat"
        ],
        "Url": "https://localhost:7001/webhooks"
      }
    ]
  }
}
```

Then, in your Program.cs setup:

```csharp
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddWebhooksCore();

services.Configure<WebhookSinksOptions>(options => configuration.GetSection("Sinks").Bind(options));
```

Broadcast a webhook event:

```csharp
var payload = new Order(); // The order that was created.

// Webhook event types are strings, which means you can use any name that you want, such as "OrderCreated"
await webhookEventBroadcaster.BroadcastAsync(new WebhookEvent("OrderCreated", payload));
```

On the receiving end, the external application needs to expose HTTP endpoints that can receive the webhook events:

```csharp
app.MapPost("/webhooks", (WebhookEvent<Order> webhookEvent) =>
{
    var order = webhookEvent.Payload!;

    if(webhookEvent.EventType == "OrderCreated")
       Console.WriteLine($"Order created event received at: {webhookEvent.Timestamp}");
});
```

This example illustrates using a strongly typed payload for the webhook event.
It's up to you to share payload types in a shared library.
Alternatively, you can use the following types: 

- JsonElement
- JsonObject
- JsonNode
- ExpandoObject
- IDictionary<string, object>

Or any other type that can be deserialized from the received payload data.
If no type is specified, the default will be a `JsonElement`.

## Glossary

```markdown
| Term   | Description                                                                                                                                                                                                                      |
|--------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Sink   | The endpoint that receives an event. In the context of webhooks, this refers to an HTTP endpoint that receives HTTP requests representing the webhook event.                                                                      |
| Source | The origin of an event. In the context of webhooks, this is the application that broadcasts an event. The webhook dispatcher then sends HTTP requests to all registered sinks.                                                   |
|        |                                                                                                                                                                                                                                  |
|        |                                                                                                                                                                                                                                  |
```

## Architecture Decision Records

Architecture decisions are documented in [docs/adr](docs/adr).

- [ADR 0001: Use GitHub Spec Kit](docs/adr/0001-use-github-spec-kit.md)

## Contributing

Looking to contribute? We'd love your help, and there are plenty of ways to get involved.

## License

This project is licensed under the [MIT License](https://github.com/sfmskywalker/webhooks-core/blob/main/LICENSE).
