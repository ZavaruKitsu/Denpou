﻿using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TelegramBotBase.Base;

namespace TelegramBotBase.Form
{
    /// <summary>
    ///     This is used to split incomming requests depending on the chat type.
    /// </summary>
    public class SplitterForm : FormBase
    {
        private static object _evOpenSupergroup = new object();
        private static object _evOpenGroup = new object();
        private static object _evOpenChannel = new object();
        private static object _evOpen = new object();


        public override async Task Load(MessageResult message)
        {
            if (message.Message.Chat.Type == ChatType.Channel)
                if (await OpenChannel(message))
                    return;
            if (message.Message.Chat.Type == ChatType.Supergroup)
            {
                if (await OpenSupergroup(message)) return;
                if (await OpenGroup(message)) return;
            }

            if (message.Message.Chat.Type == ChatType.Group)
                if (await OpenGroup(message))
                    return;

            await Open(message);
        }


        public virtual Task<bool> OpenSupergroup(MessageResult e)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OpenChannel(MessageResult e)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> Open(MessageResult e)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OpenGroup(MessageResult e)
        {
            return Task.FromResult(false);
        }


        public override Task Action(MessageResult message)
        {
            return base.Action(message);
        }

        public override Task PreLoad(MessageResult message)
        {
            return base.PreLoad(message);
        }

        public override Task Render(MessageResult message)
        {
            return base.Render(message);
        }

        public override Task SentData(DataResult message)
        {
            return base.SentData(message);
        }
    }
}