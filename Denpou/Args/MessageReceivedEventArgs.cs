using Telegram.Bot.Types;

namespace Denpou.Args;

public class MessageReceivedEventArgs
{
    public MessageReceivedEventArgs(Message m)
    {
        Message = m;
    }

    public int MessageId => Message.MessageId;

    public Message Message { get; set; }
}