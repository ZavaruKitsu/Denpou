using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Attributes;
using TelegramBotBase.Base;

namespace TelegramBotBase.Form
{
    /// <summary>
    ///     A prompt with a lot of buttons
    /// </summary>
    [IgnoreState]
    public class ArrayPromptDialog : FormBase
    {
        public ArrayPromptDialog()
        {
        }

        public ArrayPromptDialog(string Message)
        {
            this.Message = Message;
        }

        public ArrayPromptDialog(string Message, params ButtonBase[][] Buttons)
        {
            this.Message = Message;
            this.Buttons = Buttons;
        }

        /// <summary>
        ///     The message the users sees.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     An additional optional value.
        /// </summary>
        public object Tag { get; set; }

        public ButtonBase[][] Buttons { get; set; }

        [Obsolete] public Dictionary<string, FormBase> ButtonForms { get; set; } = new Dictionary<string, FormBase>();

        private EventHandlerList __Events { get; } = new EventHandlerList();

        private static object __evButtonClicked { get; } = new object();

        public override async Task Action(MessageResult message)
        {
            var call = message.GetData<CallbackData>();

            message.Handled = true;

            if (!message.IsAction)
                return;

            await message.ConfirmAction();

            await message.DeleteMessage();

            var buttons = Buttons.Aggregate((a, b) => a.Union(b).ToArray()).ToList();

            if (call == null) return;

            var button = buttons.FirstOrDefault(a => a.Value == call.Value);

            if (button == null) return;

            OnButtonClicked(new ButtonClickedEventArgs(button) { Tag = Tag });

            var fb = ButtonForms.ContainsKey(call.Value) ? ButtonForms[call.Value] : null;

            if (fb != null) await NavigateTo(fb);
        }


        public override async Task Render(MessageResult message)
        {
            var btn = new ButtonForm();

            foreach (var bl in Buttons)
                btn.AddButtonRow(
                    bl.Select(a => new ButtonBase(a.Text, CallbackData.Create("action", a.Value))).ToList());

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