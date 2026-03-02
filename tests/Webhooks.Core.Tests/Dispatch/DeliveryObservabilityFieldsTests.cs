namespace Webhooks.Core.Tests.Dispatch;

public class DeliveryObservabilityFieldsTests
{
    [Fact]
    public void DeliveryResult_Contains_Minimum_Observability_Field_Set()
    {
        var result = new DeliveryResult(
            DeliveryStatus.Failed,
            3,
            "network timeout",
            "evt-1",
            "EndpointInvoker");

        Assert.Equal(DeliveryStatus.Failed, result.Status);
        Assert.Equal(3, result.AttemptCount);
        Assert.Equal("network timeout", result.FinalFailureReason);
        Assert.Equal("evt-1", result.EventIdCorrelation);
        Assert.Equal("EndpointInvoker", result.OutcomeSource);
    }
}
