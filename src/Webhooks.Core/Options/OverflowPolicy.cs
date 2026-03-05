namespace Webhooks.Core.Options;

/// <summary>
/// Defines the behavior when the background processing queue overflows.
/// </summary>
public enum OverflowPolicy
{
    FailFast,
    DropOldest,
    Block
}
