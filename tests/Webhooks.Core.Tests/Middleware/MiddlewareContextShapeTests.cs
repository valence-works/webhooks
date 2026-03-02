namespace Webhooks.Core.Tests.Middleware;

public class MiddlewareContextShapeTests
{
    [Fact]
    public void WebhookEndpointInvocationContext_Contains_Sink_Envelope_And_Attempt()
    {
        var sink = new WebhookSink { SinkId = "sink-a", Destination = new Uri("https://example.com/a") };
        var envelope = new DeliveryEnvelope("evt-1", "order.created", null, DateTimeOffset.UtcNow);

        var context = new WebhookEndpointInvocationContext(sink, envelope, 2);

        Assert.Equal("sink-a", context.WebhookSink.SinkId);
        Assert.Equal("evt-1", context.DeliveryEnvelope.EventId);
        Assert.Equal(2, context.Attempt);
    }
}
