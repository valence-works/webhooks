namespace Webhooks.Core.Tests.Dispatch;

public class DeliveryOutcomeSemanticsTests
{
    [Fact]
    public void DeliveryResult_Defaults_OutcomeSource_To_EndpointInvoker()
    {
        var result = new DeliveryResult(DeliveryStatus.Succeeded, 1, null, "evt-1");

        Assert.Equal("EndpointInvoker", result.OutcomeSource);
    }

    [Fact]
    public void DispatchHandoffResult_Is_Separate_From_DeliveryResult_Semantics()
    {
        var handoff = new DispatchHandoffResult("default", DispatchHandoffStatus.Accepted, "evt-1");

        Assert.Equal("evt-1", handoff.EventIdCorrelation);
    }
}
