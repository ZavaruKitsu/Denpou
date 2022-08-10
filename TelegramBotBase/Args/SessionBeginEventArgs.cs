using System;
using TelegramBotBase.Sessions;

namespace TelegramBotBase.Base
{
    public class SessionBeginEventArgs : EventArgs
    {
        public SessionBeginEventArgs(long DeviceId, DeviceSession Device)
        {
            this.DeviceId = DeviceId;
            this.Device = Device;
        }

        public long DeviceId { get; set; }

        public DeviceSession Device { get; set; }
    }
}