using SpellCheckerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SpellCheckerBot.Bot;

public class BotManagement
{
    private readonly TelegramBotClient _bot;

    public BotManagement(TelegramBotClient bot) => _bot = bot;

    public async Task AddCommand(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        var currentCommands = await _bot.GetMyCommands();
        BotCommand command = (name.Prepend('/').ToString()!, description);
        await _bot.SetMyCommands([.. currentCommands, command]);
    }

    public async Task<IEnumerable<CommandDetails>> GetCommandsInfo()
    {
        var commands = await _bot.GetMyCommands();
        return commands.Select(c => new CommandDetails { Name = c.Command, Description = c.Description });
    }

    public async Task<bool> IsCommand(string commandName)
    {
        var commands = await _bot.GetMyCommands();
        return commands.Any(c => c.Command == commandName);
    }
}