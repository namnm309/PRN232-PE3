using System.Text.Json;

namespace Q2;

internal static class JsonSerializerDefaults
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
