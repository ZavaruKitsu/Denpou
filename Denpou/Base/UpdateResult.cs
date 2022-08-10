using Denpou.Sessions;
using Telegram.Bot.Types;

namespace Denpou.Base;

public class UpdateResult : ResultBase
{
    public UpdateResult(Update rawData, DeviceSession device)
    {
        RawData = rawData;
        Device = device;
    }

    /// <summary>
    ///     Returns the Device/ChatId
    /// </summary>
    public override long DeviceId =>
        RawData?.Message?.Chat?.Id
        ?? RawData?.CallbackQuery?.Message?.Chat?.Id
        ?? Device?.DeviceId
        ?? 0;

    public Update RawData { get; set; }

    public override Message Message =>
        RawData?.Message
        ?? RawData?.EditedMessage
        ?? RawData?.ChannelPost
        ?? RawData?.EditedChannelPost
        ?? RawData?.CallbackQuery?.Message;


    public DeviceSession Device { get; set; }
}