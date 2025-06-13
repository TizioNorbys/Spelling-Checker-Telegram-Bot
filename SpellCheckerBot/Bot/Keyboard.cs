using System.Text.Json;
using SpellCheckerBot.Bot.Callback.Data;
using SpellCheckerBot.SpellCheckerApi.Models;
using SpellCheckerBot.Utility;
using Telegram.Bot.Types.ReplyMarkups;

namespace SpellCheckerBot.Bot;

public static class Keyboard
{
    private const int LanguagesCallbackQueryId = 1;
    private const int ReplacementsCallbackQueryId = 2;

    public static InlineKeyboardMarkup LanguagesKeyboard { get; }

    private static readonly Dictionary<string, string> languages = JsonReader.ReadAndDeserializeToDictionary<string>("Bot/Data/Languages.json");
    public static IReadOnlyDictionary<string, string> Languages
    {
        get
        {
            if (languages.Count == 0)
                throw new Exception("Failed to build languages dictionary");
            return languages;
        }
    }

    static Keyboard()
    {
        LanguagesKeyboard = BuildLanguagesKeyboard();
    }

    private static InlineKeyboardMarkup BuildLanguagesKeyboard()
    {
        var keyboard = new InlineKeyboardMarkup();

        int count = 0;
        foreach (var pair in languages)
        {
            if (count % 2 == 0)
                keyboard.AddNewRow();

            string languageCode = pair.Key;
            var languageDetails = new LanguageDetails(LanguagesCallbackQueryId, languageCode);
            string json = JsonSerializer.Serialize(languageDetails);

            keyboard.AddButton(pair.Value, json);
            count++;
        }

        return keyboard;
    }

    public static InlineKeyboardMarkup BuildReplacementsKeyboard(Match match, long messageId)
    {
        var keyboard = new InlineKeyboardMarkup();

        for (int i = 0; i < match.Replacements.Take(6).Count(); i++)
        {
            if (i == 3)
                keyboard.AddNewRow();
            
            string value = match.Replacements[i].Value;
            var matchDetails = new MatchDetails(ReplacementsCallbackQueryId, messageId, value, match.Offset, match.Length);
            string json = JsonSerializer.Serialize(matchDetails);

            keyboard.AddButton(value, json);
        }

        return keyboard;
    }
}