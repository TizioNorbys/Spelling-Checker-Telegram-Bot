using System.Text.Json;

namespace SpellCheckerBot.Utility;

public static class JsonReader
{
    private static readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

    public static Dictionary<string, TValue> ReadAndDeserializeToDictionary<TValue>(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Dictionary<string, TValue>>(json, options) ?? new Dictionary<string, TValue>();
    }
}