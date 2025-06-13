namespace SpellCheckerBot.Bot.Models;

public record class CommandDetails
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}