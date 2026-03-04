using System.Text.Json;

namespace Webhooks.Core;

public interface IPayloadFieldSelectorStrategy
{
    bool TrySelect(JsonElement payload, string selector, out string? selectedValue);
}
