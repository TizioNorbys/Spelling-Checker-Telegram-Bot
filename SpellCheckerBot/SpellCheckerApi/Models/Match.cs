namespace SpellCheckerBot.SpellCheckerApi.Models;

public record Match(
    string Message,
    string ShortMessage,
    IList<Replacement> Replacements,
    int Offset,
    int Length,
    Context Context,
    Rule Rule)
{
    public string GetMatchDescription()
    {
        string description = !string.IsNullOrWhiteSpace(ShortMessage) ? ShortMessage : Message;
        string mistake = Context.Text[Context.Offset..(Context.Offset + Context.Length)];
        return $"{description} -> <i>{mistake}</i>";
    }
};