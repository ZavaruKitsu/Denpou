﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotBase.Args;
using TelegramBotBase.Attributes;
using TelegramBotBase.Base;
using TelegramBotBase.Localizations;

namespace TelegramBotBase.Form
{
    [IgnoreState]
    public class PromptDialog : ModalDialog
    {
        public PromptDialog()
        {
        }

        public PromptDialog(string Message)
        {
            this.Message = Message;
        }

        /// <summary>
        ///     The message the users sees.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     The returned text value by the user.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     An additional optional value.
        /// </summary>
        public object Tag { get; set; }

        private EventHandlerList __Events { get; } = new EventHandlerList();

        private static object __evCompleted { get; } = new object();

        public bool ShowBackButton { get; set; } = false;

        public string BackLabel { get; set; } = Default.Language["PromptDialog_Back"];

        /// <summary>
        ///     Contains the RAW received message.
        /// </summary>
        public Message ReceivedMessage { get; set; }

        public override async Task Load(MessageResult message)
        {
            if (message.Handled)
                return;

            if (!message.IsFirstHandler)
                return;

            if (ShowBackButton && message.MessageText == BackLabel)
            {
                await CloseForm();

                return;
            }

            if (Value == null)
            {
                Value = message.MessageText;

                ReceivedMessage = message.Message;
            }
        }

        public override async Task Render(MessageResult message)
        {
            if (Value == null)
            {
                if (ShowBackButton)
                {
                    var bf = new ButtonForm();
                    bf.AddButtonRow(new ButtonBase(BackLabel, "back"));
                    await Device.Send(Message, (ReplyMarkupBase)bf);
                    return;
                }

                await Device.Send(Message);
                return;
            }


            message.Handled = true;

            OnCompleted(new PromptDialogCompletedEventArgs { Tag = Tag, Value = Value });

            await CloseForm();
        }


        public event EventHandler<PromptDialogCompletedEventArgs> Completed
        {
            add => __Events.AddHandler(__evCompleted, value);
            remove => __Events.RemoveHandler(__evCompleted, value);
        }

        public void OnCompleted(PromptDialogCompletedEventArgs e)
        {
            (__Events[__evCompleted] as EventHandler<PromptDialogCompletedEventArgs>)?.Invoke(this, e);
        }
    }
}