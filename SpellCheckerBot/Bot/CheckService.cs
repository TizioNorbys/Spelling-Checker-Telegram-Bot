using SpellCheckerBot.Bot.Callback.Data;

namespace SpellCheckerBot.Bot.Check;

public class CheckService
{
    public const string WhiteSpaceRuleId = "WHITESPACE_RULE";

    private readonly Dictionary<long, long> toUpdateMessagesIds = [];
    private readonly Dictionary<long, string> toUpdateMessages = [];

    public IReadOnlyDictionary<long, long> ToUpdateMessagesIds => toUpdateMessagesIds;
    public IReadOnlyDictionary<long, string> ToUpdatedMessages => toUpdateMessages;  
    
    public void AddOrUpdateToUpdateMessageId(long userId, long messageId)
    {
        if (toUpdateMessagesIds.TryAdd(userId, messageId))
            return;
        
        if (toUpdateMessagesIds[userId] == messageId)
            return;

        toUpdateMessagesIds[userId] = messageId;
    }

    public void AddOrUpdateToUpdateMessage(long userId, string messageText)
    {
        if (toUpdateMessages.TryAdd(userId, messageText))
            return;

        if (toUpdateMessages[userId] == messageText)
            return;

        toUpdateMessages[userId] = messageText;
    }

    public string UpdateMessage(MatchDetails match, long userId)
    {
        string message = toUpdateMessages[userId];
        string updatedMessage = message.Remove(match.O, match.L).Insert(match.O, match.R);
        toUpdateMessages[userId] = updatedMessage;

        return updatedMessage;
    }
}