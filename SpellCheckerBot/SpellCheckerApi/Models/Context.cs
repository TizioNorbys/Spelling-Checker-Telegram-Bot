namespace SpellCheckerBot.SpellCheckerApi.Models;

public record Context(string Text, int Offset, int Length);