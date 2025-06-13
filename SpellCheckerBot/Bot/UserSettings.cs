namespace SpellCheckerBot.Bot;

public static class UserSettings
{
    public const string DefaultLanguage = "auto";
    private static readonly Dictionary<long, string> usersLanguagesCodes = [];

    public static void AddOrUpdate(long userId, string languageCode)
    {
        if (usersLanguagesCodes.TryAdd(userId, languageCode))
            return;

        if (usersLanguagesCodes[userId] == languageCode)
            return;

        usersLanguagesCodes[userId] = languageCode;
    }

    public static string GetOrDefault(long userId)
    {
        return usersLanguagesCodes.TryGetValue(userId, out string? languageCode) ? languageCode : DefaultLanguage;
    }
}