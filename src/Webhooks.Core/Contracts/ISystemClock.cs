namespace Webhooks.Core;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}