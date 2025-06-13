using System.Text.Json;
using SpellCheckerBot.Bot.Callback.Data.Base;
using Telegram.Bot.Types;

namespace SpellCheckerBot.Utility;

public static class CallbackQueryHelper
{
    public static TQueryData? GetQueryData<TQueryData>(CallbackQuery query)
        where TQueryData : CallbackDataBase
    {
        if (query.Data is null)
            return null;

        string json = query.Data;
        var callbackData = JsonSerializer.Deserialize<TQueryData>(json);

        return callbackData;
    }

    public static int? GetQueryId(CallbackQuery query)
    {
        if (query.Data is null)
            return null;

        string json = query.Data;
        var callbackData = JsonSerializer.Deserialize<CallbackDataBase>(json)!;

        return callbackData.QId == default ? null : callbackData.QId;
    }
}