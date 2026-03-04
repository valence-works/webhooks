using Webhooks.Core.Options;

namespace Webhooks.Core.Tests.Dispatch;

public class QueueCapacityAndParallelismTests
{
    [Fact]
    public void Validate_Fails_When_QueueCapacity_Is_Negative()
    {
        var validator = new ConfigureWebhookEventBroadcasterOptions();
        var options = new WebhookBroadcasterOptions { QueueCapacity = -1 };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_Fails_When_WorkerParallelism_Is_Negative()
    {
        var validator = new ConfigureWebhookEventBroadcasterOptions();
        var options = new WebhookBroadcasterOptions { WorkerParallelism = -1 };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }
}
