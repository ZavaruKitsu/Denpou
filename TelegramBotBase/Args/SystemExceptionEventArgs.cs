using System;
using TelegramBotBase.Sessions;

namespace TelegramBotBase.Args
{
    public class SystemExceptionEventArgs : EventArgs
    {
        public SystemExceptionEventArgs()
        {
        }

        public SystemExceptionEventArgs(string Command, long DeviceId, DeviceSession Device, Exception error)
        {
            this.Command = Command;
            this.DeviceId = DeviceId;
            this.Device = Device;
            Error = error;
        }

        public string Command { get; set; }

        public long DeviceId { get; set; }

        public DeviceSession Device { get; set; }

        public Exception Error { get; set; }
    }
}