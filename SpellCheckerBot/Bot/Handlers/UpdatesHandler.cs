using SpellCheckerBot.Bot.Callback.Data;
using SpellCheckerBot.Bot.Check;
using SpellCheckerBot.Bot.Handlers.Base;
using SpellCheckerBot.SpellCheckerApi;
using SpellCheckerBot.Utility;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SpellCheckerBot.Bot.Handlers;

public class UpdatesHandler : HandlerBase
{
    private readonly CheckService _checkService;

    public UpdatesHandler(TelegramBotClient bot, LanguageToolClient spellCheckerClient, CheckService checkService)
        : base(bot, spellCheckerClient)
    {
        _checkService = checkService;
    }

    public async Task HandleLanguageCallbackQueryAsync(CallbackQuery query)
    {
        var callbackData = CallbackQueryHelper.GetQueryData<LanguageDetails>(query)!;
        string languageCode = callbackData.LanguageCode;

        UserSettings.AddOrUpdate(query.From.Id, languageCode);
        await _bot.SendMessage(query.Message!.Chat.Id, $"Your language is set to \t{Keyboard.Languages[languageCode]}.");
    }

    public async Task HandleReplacementCallbackQueryAsync(CallbackQuery query)
    {
        long chatId = query.Message!.Chat.Id;
        long userId = query.From.Id;

        var callbackData = CallbackQueryHelper.GetQueryData<MatchDetails>(query)!;
        if (callbackData.MId != _checkService.ToUpdateMessagesIds[userId])
            return;

        string updatedMessage = _checkService.UpdateMessage(callbackData, userId);
        Message message = await _bot.SendMessage(chatId, updatedMessage);
        await _bot.DeleteMessage(chatId, query.Message.Id);
        _checkService.AddOrUpdateToUpdateMessageId(userId, message.Id);

        string userLanguageCode = UserSettings.GetOrDefault(userId);

        var check = await _spellCheckerClient.GetCheckAsync(updatedMessage, userLanguageCode);
        if (check is null)
        {
            await _bot.SendMessage(chatId, "Bot error, try again later");
            return;
        }

        if (check.Matches.Count == 0)
        {
            await _bot.SendMessage(chatId, "Great! The text has no mistakes.");
            return;
        }

        foreach (var match in check.Matches.Where(m => m.Rule.Id != CheckService.WhiteSpaceRuleId))
        {
            await _bot.SendMessage(
                chatId,
                match.GetMatchDescription(),
                parseMode: ParseMode.Html,
                replyMarkup: Keyboard.BuildReplacementsKeyboard(match, message.Id));
        }
    }
}