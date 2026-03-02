using Webhooks.Core.Options;

namespace Webhooks.Core.Tests.Validation;

public class SinkRegistrationValidationTests
{
    [Fact]
    public void Validate_Fails_When_SinkId_Is_Duplicate()
    {
        var validator = new ValidateWebhookSinksOptions();
        var options = new WebhookSinksOptions
        {
            Sinks = new List<WebhookSink>
            {
                CreateSink("sink-a", "order.created"),
                CreateSink("sink-a", "invoice.created")
            }
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_Fails_When_Destination_Is_Missing()
    {
        var validator = new ValidateWebhookSinksOptions();
        var options = new WebhookSinksOptions
        {
            Sinks = new List<WebhookSink>
            {
                new()
                {
                    SinkId = "sink-a",
                    Subscriptions = new List<WebhookEventFilter>
                    {
                        new() { EventType = "order.created" }
                    }
                }
            }
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_Fails_When_Subscriptions_Are_Missing()
    {
        var validator = new ValidateWebhookSinksOptions();
        var options = new WebhookSinksOptions
        {
            Sinks = new List<WebhookSink>
            {
                new()
                {
                    SinkId = "sink-a",
                    Destination = new Uri("https://example.com/a")
                }
            }
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    private static WebhookSink CreateSink(string id, string eventType)
    {
        return new WebhookSink
        {
            SinkId = id,
            Destination = new Uri($"https://example.com/{id}"),
            Subscriptions = new List<WebhookEventFilter>
            {
                new() { EventType = eventType }
            }
        };
    }
}
