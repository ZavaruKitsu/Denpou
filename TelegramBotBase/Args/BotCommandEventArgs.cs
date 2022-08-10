using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using TelegramBotBase.Sessions;

namespace TelegramBotBase.Args
{
    /// <summary>
    ///     Base class for given bot command results
    /// </summary>
    public class BotCommandEventArgs : EventArgs
    {
        public BotCommandEventArgs()
        {
        }

        public BotCommandEventArgs(string Command, List<string> Parameters, Message Message, long DeviceId,
            DeviceSession Device)
        {
            this.Command = Command;
            this.Parameters = Parameters;
            OriginalMessage = Message;
            this.DeviceId = DeviceId;
            this.Device = Device;
        }

        public string Command { get; set; }

        public List<string> Parameters { get; set; }

        public long DeviceId { get; set; }

        public DeviceSession Device { get; set; }

        public bool Handled { get; set; } = false;

        public Message OriginalMessage { get; set; }
    }
}