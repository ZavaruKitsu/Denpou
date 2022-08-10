using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TelegramBotBase.Base;
using TelegramBotBase.Form;
using TelegramBotBase.Localizations;

namespace TelegramBotBase.Controls.Inline
{
    public class ToggleButton : ControlBase
    {
        private static readonly object __evToggled = new object();

        private readonly EventHandlerList Events = new EventHandlerList();

        private bool RenderNecessary = true;


        public ToggleButton()
        {
        }

        public ToggleButton(string CheckedString, string UncheckedString)
        {
            this.CheckedString = CheckedString;
            this.UncheckedString = UncheckedString;
        }

        public string UncheckedIcon { get; set; } = Default.Language["ToggleButton_OffIcon"];

        public string CheckedIcon { get; set; } = Default.Language["ToggleButton_OnIcon"];

        public string CheckedString { get; set; } = Default.Language["ToggleButton_On"];

        public string UncheckedString { get; set; } = Default.Language["ToggleButton_Off"];

        public string ChangedString { get; set; } = Default.Language["ToggleButton_Changed"];

        public string Title { get; set; } = Default.Language["ToggleButton_Title"];

        public int? MessageId { get; set; }

        public bool Checked { get; set; }

        public event EventHandler Toggled
        {
            add => Events.AddHandler(__evToggled, value);
            remove => Events.RemoveHandler(__evToggled, value);
        }

        public void OnToggled(EventArgs e)
        {
            (Events[__evToggled] as EventHandler)?.Invoke(this, e);
        }

        public override async Task Action(MessageResult result, string value = null)
        {
            if (result.Handled)
                return;

            await result.ConfirmAction(ChangedString);

            switch (value ?? "unknown")
            {
                case "on":

                    if (Checked)
                        return;

                    RenderNecessary = true;

                    Checked = true;

                    OnToggled(new EventArgs());

                    break;

                case "off":

                    if (!Checked)
                        return;

                    RenderNecessary = true;

                    Checked = false;

                    OnToggled(new EventArgs());

                    break;

                default:

                    RenderNecessary = false;

                    break;
            }

            result.Handled = true;
        }

        public override async Task Render(MessageResult result)
        {
            if (!RenderNecessary)
                return;

            var bf = new ButtonForm(this);

            var bOn = new ButtonBase((Checked ? CheckedIcon : UncheckedIcon) + " " + CheckedString, "on");

            var bOff = new ButtonBase((!Checked ? CheckedIcon : UncheckedIcon) + " " + UncheckedString, "off");

            bf.AddButtonRow(bOn, bOff);

            if (MessageId != null)
            {
                var m = await Device.Edit(MessageId.Value, Title, bf);
            }
            else
            {
                var m = await Device.Send(Title, bf, disableNotification: true);
                if (m != null) MessageId = m.MessageId;
            }

            RenderNecessary = false;
        }
    }
}