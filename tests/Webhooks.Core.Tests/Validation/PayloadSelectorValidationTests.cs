using Webhooks.Core.Options;

namespace Webhooks.Core.Tests.Validation;

public class PayloadSelectorValidationTests
{
    [Fact]
    public void Validate_Fails_For_Invalid_Payload_Selector_Syntax()
    {
        var validator = new ValidateWebhookSinksOptions();
        var options = new WebhookSinksOptions
        {
            Sinks = new List<WebhookSink>
            {
                new()
                {
                    SinkId = "sink-a",
                    Destination = new Uri("https://example.com/a"),
                    Subscriptions = new List<WebhookEventFilter>
                    {
                        new()
                        {
                            EventType = "order.created",
                            PayloadMatchingMode = PayloadMatchingMode.And,
                            PayloadFilters = new List<PayloadFilter>
                            {
                                new("customer.id", "123")
                            }
                        }
                    }
                }
            }
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }
}
