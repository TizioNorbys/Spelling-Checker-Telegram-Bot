using SpellCheckerBot.Bot;
using SpellCheckerBot.Bot.Check;
using SpellCheckerBot.SpellCheckerApi;
using Telegram.Bot;

namespace SpellCheckerBot;

class Program
{
    static void Main(string[] args)
    {
        var token = Environment.GetEnvironmentVariable("SpellCheckerBotToken")!;
        using var cts = new CancellationTokenSource();

        var bot = new TelegramBotClient(token, cancellationToken: cts.Token);
        BotService botService = new(bot, new LanguageToolClient(), new CheckService());

        Console.ReadKey();
        cts.Cancel();
    }
}