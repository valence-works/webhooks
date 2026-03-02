using System.Text.Json;
using System.Text.RegularExpressions;

namespace Webhooks.Core.Strategies;

public class JsonPathPayloadFieldSelectorStrategy : IPayloadFieldSelectorStrategy
{
    private static readonly Regex SelectorRegex = new("^\\$([.][A-Za-z_][A-Za-z0-9_]*)+$", RegexOptions.Compiled);

    public bool TrySelect(JsonElement payload, string selector, out string? selectedValue)
    {
        selectedValue = null;
        if (!SelectorRegex.IsMatch(selector))
        {
            return false;
        }

        var cursor = payload;
        var segments = selector[2..].Split('.', StringSplitOptions.RemoveEmptyEntries);
        foreach (var segment in segments)
        {
            if (!cursor.TryGetProperty(segment, out var next))
            {
                return false;
            }

            cursor = next;
        }

        selectedValue = cursor.ValueKind == JsonValueKind.String ? cursor.GetString() : cursor.ToString();
        return true;
    }
}
