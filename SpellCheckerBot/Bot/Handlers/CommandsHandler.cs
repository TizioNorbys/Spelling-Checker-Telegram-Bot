using System.Text;
using SpellCheckerBot.Bot.Check;
using SpellCheckerBot.Bot.Handlers.Base;
using SpellCheckerBot.SpellCheckerApi;
using SpellCheckerBot.SpellCheckerApi.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SpellCheckerBot.Bot.Handlers;

public class CommandsHandler : HandlerBase
{
    private readonly string languagesList;
    private readonly CheckService _checkService;

    public CommandsHandler(TelegramBotClient bot, LanguageToolClient spellCheckerClient, CheckService checkService)
        : base(bot, spellCheckerClient)
    {
        languagesList = BuildLanguagesList().Result;
        _checkService = checkService;
    }

    private async Task<string> BuildLanguagesList()
    {
        var languagesDetails = await _spellCheckerClient.GetSupportedLanguagesAsync();
        
        StringBuilder languages = new();
        foreach (var languageDetail in languagesDetails.SkipLast(9))
        {
            languages.AppendLine($"{languageDetail.Name}");
        }

        return languages.ToString();
    }

    public async Task HandleStartCommandAsync(Message message)
    {
        string messageText = "Hi, welcome to <b>Spell Checker Bot</b>!\n\nSend the text you'd like to check: the bot will analyze it and provide corrections for any spelling mistakes it detects.\n\nYou can set your language using <i>/setlanguage</i>, otherwise the bot will try to guess it automatically.";
        await _bot.SendMessage(message.Chat.Id, messageText, parseMode: ParseMode.Html);
    }

    public async Task HandleLanguagesCommandAsync(Message message)
    {
        if (languagesList.Length == 0)
        {
            await _bot.SendMessage(message.Chat.Id, "Bot error, try again later");
            return;
        }

        await _bot.SendMessage(message.Chat.Id, "The bot support these languages:\n\n" + languagesList);
    }

    public async Task HandleSetLanguageCommandAsync(Message message)
    {
        await _bot.SendMessage(message.Chat.Id, "Choose your language:", replyMarkup: Keyboard.LanguagesKeyboard);
    }

    public async Task HandleCheckAsync(Message message)
    {
        string userLanguageCode = UserSettings.GetOrDefault(message.From!.Id);

        var check = await _spellCheckerClient.GetCheckAsync(message.Text!, userLanguageCode);
        if (check is null)
        {
            await _bot.SendMessage(message.Chat.Id, "Bot error, try again later");
            return;
        }

        if (check.Matches.Count == 0)
        {
            await _bot.SendMessage(message.Chat.Id, "Great! The text has no mistakes.");
            return;
        }

        _checkService.AddOrUpdateToUpdateMessageId(message.From.Id, message.Id);
        _checkService.AddOrUpdateToUpdateMessage(message.From.Id, message.Text!);

        Func<Match, bool> filter = m => m.Rule.Id != CheckService.WhiteSpaceRuleId;
        await _bot.SendMessage(message.Chat.Id, $"{check.Matches.Where(filter).Count()} potential mistakes found \t⚠️");

        foreach (var match in check.Matches.Where(filter))
        {
            await _bot.SendMessage(
                message.Chat.Id,
                match.GetMatchDescription(),
                parseMode: ParseMode.Html,
                replyMarkup: Keyboard.BuildReplacementsKeyboard(match, message.Id));
        }
    }
}