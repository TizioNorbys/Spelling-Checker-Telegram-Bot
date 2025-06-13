using SpellCheckerBot.Bot.Callback.Data.Base;

namespace SpellCheckerBot.Bot.Callback.Data;

public sealed record MatchDetails(int QId, long MId, string R, int O, int L) // queryId, MessageId, Replacement, Offset, Length
    : CallbackDataBase(QId);