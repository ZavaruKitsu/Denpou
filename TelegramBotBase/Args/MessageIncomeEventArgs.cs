using System;
using TelegramBotBase.Sessions;

namespace TelegramBotBase.Base
{
    public class MessageIncomeEventArgs : EventArgs
    {
        public MessageIncomeEventArgs(long DeviceId, DeviceSession Device, MessageResult message)
        {
            this.DeviceId = DeviceId;
            this.Device = Device;
            Message = message;
        }

        public long DeviceId { get; set; }

        public DeviceSession Device { get; set; }

        public MessageResult Message { get; set; }
    }
}