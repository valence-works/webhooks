namespace Webhooks.Core.Services;

/// <summary>
/// Default system clock that returns <see cref="DateTimeOffset.UtcNow"/>.
/// </summary>
public sealed class DefaultSystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}