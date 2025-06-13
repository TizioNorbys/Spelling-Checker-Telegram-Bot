using SpellCheckerBot.SpellCheckerApi;
using Telegram.Bot;

namespace SpellCheckerBot.Bot.Handlers.Base;

public abstract class HandlerBase
{
    protected readonly TelegramBotClient _bot;
    protected readonly LanguageToolClient _spellCheckerClient;

    protected HandlerBase(TelegramBotClient bot, LanguageToolClient spellCheckerClient)
    {
        _bot = bot;
        _spellCheckerClient = spellCheckerClient;
    }
}