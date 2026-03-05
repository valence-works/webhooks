using Webhooks.Core.Options;

namespace Webhooks.Core.Tests.Validation;

public sealed class DispatcherRegistrationValidationTests
{
    [Fact]
    public void Validate_Fails_When_DefaultDispatcher_Is_Empty()
    {
        var validator = new ConfigureWebhookEventBroadcasterOptions();
        var options = new WebhookBroadcasterOptions
        {
            DefaultDispatcher = string.Empty
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_Fails_When_QueueCapacity_Is_Zero()
    {
        var validator = new ConfigureWebhookEventBroadcasterOptions();
        var options = new WebhookBroadcasterOptions
        {
            QueueCapacity = 0
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }
}
