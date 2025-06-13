using SpellCheckerBot.Bot.Callback.Data.Base;

namespace SpellCheckerBot.Bot.Callback.Data;

public sealed record LanguageDetails(int QId, string LanguageCode)
    : CallbackDataBase(QId);