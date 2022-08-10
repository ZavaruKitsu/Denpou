using System;
using Telegram.Bot.Types;
using TelegramBotBase.Sessions;

namespace TelegramBotBase.Args
{
    public class UnhandledCallEventArgs : EventArgs
    {
        public UnhandledCallEventArgs()
        {
            Handled = false;
        }

        public UnhandledCallEventArgs(string Command, string RawData, long DeviceId, int MessageId, Message message,
            DeviceSession Device) : this()
        {
            this.Command = Command;
            this.RawData = RawData;
            this.DeviceId = DeviceId;
            this.MessageId = MessageId;
            Message = message;
            this.Device = Device;
        }

        public string Command { get; set; }

        public long DeviceId { get; set; }

        public DeviceSession Device { get; set; }

        public string RawData { get; set; }

        public int MessageId { get; set; }

        public Message Message { get; set; }

        public bool Handled { get; set; }
    }
}