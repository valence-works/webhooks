namespace Webhooks.Core.Tests.Dispatch;

public sealed class DispatchExceptionTaxonomyTests
{
    [Fact]
    public void DispatchException_Exposes_Exception_Kind()
    {
        var exception = new DispatchException("dispatcher unavailable", DispatchExceptionKind.DispatcherUnavailable);

        Assert.Equal(DispatchExceptionKind.DispatcherUnavailable, exception.Kind);
        Assert.Equal("dispatcher unavailable", exception.Message);
    }
}
