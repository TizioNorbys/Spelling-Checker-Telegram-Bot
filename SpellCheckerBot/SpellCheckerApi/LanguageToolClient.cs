using System.Text.Json;
using SpellCheckerBot.SpellCheckerApi.Models;

namespace SpellCheckerBot.SpellCheckerApi;

public class LanguageToolClient
{
    private static readonly HttpClient globalClient;
    private static readonly JsonSerializerOptions serializerOptions;
    private readonly HttpClient client;

    static LanguageToolClient()
    {
        globalClient = new HttpClient() { BaseAddress = new("https://api.languagetoolplus.com/v2/") };
        serializerOptions = new() { PropertyNameCaseInsensitive = true };
    }

    public LanguageToolClient()
    {
        client = globalClient;
    }

    public async Task<IEnumerable<Language>> GetSupportedLanguagesAsync()
    {
        var response = await client.GetAsync("languages");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Language>>(json, serializerOptions) ?? Enumerable.Empty<Language>();
    }

    public async Task<Check?> GetCheckAsync(string textToBeChecked, string languageCode)
    {
        var content = new FormUrlEncodedContent(
        [
            new("text", textToBeChecked),
            new("language", languageCode)
        ]);

        var response = await client.PostAsync("check", content);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Check>(json, serializerOptions);
    }
}