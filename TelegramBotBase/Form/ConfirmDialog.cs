﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Attributes;
using TelegramBotBase.Base;

namespace TelegramBotBase.Form
{
    [IgnoreState]
    public class ConfirmDialog : ModalDialog
    {
        public ConfirmDialog()
        {
        }

        public ConfirmDialog(string Message)
        {
            this.Message = Message;
            Buttons = new List<ButtonBase>();
        }

        public ConfirmDialog(string Message, params ButtonBase[] Buttons)
        {
            this.Message = Message;
            this.Buttons = Buttons.ToList();
        }

        /// <summary>
        ///     The message the users sees.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     An additional optional value.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     Automatically close form on button click
        /// </summary>
        public bool AutoCloseOnClick { get; set; } = true;

        public List<ButtonBase> Buttons { get; set; }

        private EventHandlerList __Events { get; } = new EventHandlerList();

        private static object __evButtonClicked { get; } = new object();

        /// <summary>
        ///     Adds one Button
        /// </summary>
        /// <param name="button"></param>
        public void AddButton(ButtonBase button)
        {
            Buttons.Add(button);
        }

        public override async Task Action(MessageResult message)
        {
            if (message.Handled)
                return;

            if (!message.IsFirstHandler)
                return;

            var call = message.GetData<CallbackData>();
            if (call == null)
                return;

            message.Handled = true;

            await message.ConfirmAction();

            await message.DeleteMessage();

            var button = Buttons.FirstOrDefault(a => a.Value == call.Value);

            if (button == null) return;

            OnButtonClicked(new ButtonClickedEventArgs(button) { Tag = Tag });

            if (AutoCloseOnClick)
                await CloseForm();
        }


        public override async Task Render(MessageResult message)
        {
            var btn = new ButtonForm();

            var buttons = Buttons.Select(a => new ButtonBase(a.Text, CallbackData.Create("action", a.Value))).ToList();
            btn.AddButtonRow(buttons);

            await Device.Send(Message, btn);
        }


        public event EventHandler<ButtonClickedEventArgs> ButtonClicked
        {
            add => __Events.AddHandler(__evButtonClicked, value);
            remove => __Events.RemoveHandler(__evButtonClicked, value);
        }

        public void OnButtonClicked(ButtonClickedEventArgs e)
        {
            (__Events[__evButtonClicked] as EventHandler<ButtonClickedEventArgs>)?.Invoke(this, e);
        }
    }
}