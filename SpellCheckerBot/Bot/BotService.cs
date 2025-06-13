using SpellCheckerBot.Bot.Callback;
using SpellCheckerBot.Bot.Check;
using SpellCheckerBot.Bot.Handlers;
using SpellCheckerBot.SpellCheckerApi;
using SpellCheckerBot.Utility;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SpellCheckerBot.Bot;

public class BotService
{
    private readonly TelegramBotClient _bot;
    private readonly CommandsHandler commandsHandler;
    private readonly UpdatesHandler updatesHandler;

    public BotService(TelegramBotClient bot, LanguageToolClient spellCheckerClient, CheckService checkService)
    {
        _bot = bot;
        commandsHandler = new(bot, spellCheckerClient, checkService);
        updatesHandler = new(bot, spellCheckerClient, checkService);

        _bot.OnMessage += Bot_OnMessage;
        _bot.OnUpdate += Bot_OnUpdate;
    }

    private async Task Bot_OnMessage(Message message, UpdateType type)
    {
        if (message.Type is not MessageType.Text)
        {
            await _bot.SendMessage(message.Chat.Id, "Send a valid command to interact with the bot");
            return;
        }

        switch (message.Text)
        {
            case "/start":
                await commandsHandler.HandleStartCommandAsync(message);
                break;

            case "/languages":
                await commandsHandler.HandleLanguagesCommandAsync(message);
                break;

            case "/setlanguage":
                await commandsHandler.HandleSetLanguageCommandAsync(message);
                break;
            default:
                await commandsHandler.HandleCheckAsync(message);
                break;
        }
    }

    private async Task Bot_OnUpdate(Update update)
    {
        if (update is { CallbackQuery: { } query})
        {
            if (query.Data is null)
                return;

            var queryId = CallbackQueryHelper.GetQueryId(query);
            switch (queryId)
            {
                case (int)CallbackQueryId.Languges:
                    await updatesHandler.HandleLanguageCallbackQueryAsync(query);
                    break;
                case (int)CallbackQueryId.Replacements:
                    await updatesHandler.HandleReplacementCallbackQueryAsync(query);
                    break;
                default:
                    break;
            }
        }
    }
}