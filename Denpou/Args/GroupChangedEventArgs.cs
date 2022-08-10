using System;
using Denpou.Base;
using Telegram.Bot.Types.Enums;

namespace Denpou.Args;

public class GroupChangedEventArgs : EventArgs
{
    public GroupChangedEventArgs(MessageType type, MessageResult message)
    {
        Type = type;
        OriginalMessage = message;
    }

    public MessageType Type { get; set; }

    public MessageResult OriginalMessage { get; set; }
}