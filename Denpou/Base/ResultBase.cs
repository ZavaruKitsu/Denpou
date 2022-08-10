using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Denpou.Base;

public class ResultBase : EventArgs
{
    public MessageClient Client { get; set; }

    public virtual long DeviceId { get; set; }

    public int MessageId => Message.MessageId;

    public virtual Message Message { get; set; }

    /// <summary>
    ///     Deletes the current message
    /// </summary>
    /// <returns></returns>
    public virtual async Task DeleteMessage()
    {
        await DeleteMessage(MessageId);
    }

    /// <summary>
    ///     Deletes the current message or the given one.
    /// </summary>
    /// <param name="messageId"></param>
    /// <returns></returns>
    public virtual async Task DeleteMessage(int messageId = -1)
    {
        try
        {
            await Client.TelegramClient.DeleteMessageAsync(DeviceId, messageId == -1 ? MessageId : messageId);
        }
        catch
        {
        }
    }
}