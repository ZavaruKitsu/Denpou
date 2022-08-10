using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TelegramBotBase.Base;
using TelegramBotBase.Form;
using TelegramBotBase.Localizations;

namespace TelegramBotBase.Controls.Inline
{
    public class MultiToggleButton : ControlBase
    {
        private static readonly object __evToggled = new object();

        private readonly EventHandlerList Events = new EventHandlerList();

        private bool RenderNecessary = true;


        public MultiToggleButton()
        {
            Options = new List<ButtonBase>();
        }

        /// <summary>
        ///     This contains the selected icon.
        /// </summary>
        public string SelectedIcon { get; set; } = Default.Language["MultiToggleButton_SelectedIcon"];

        /// <summary>
        ///     This will appear on the ConfirmAction message (if not empty)
        /// </summary>
        public string ChangedString { get; set; } = Default.Language["MultiToggleButton_Changed"];

        /// <summary>
        ///     This holds the title of the control.
        /// </summary>
        public string Title { get; set; } = Default.Language["MultiToggleButton_Title"];

        public int? MessageId { get; set; }

        /// <summary>
        ///     This will hold all options available.
        /// </summary>
        public List<ButtonBase> Options { get; set; }

        /// <summary>
        ///     This will set if an empty selection (null) is allowed.
        /// </summary>
        public bool AllowEmptySelection { get; set; } = true;

        public ButtonBase SelectedOption { get; set; }

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
                default:

                    var s = value.Split('$');

                    if (s[0] == "check" && s.Length > 1)
                    {
                        var index = 0;
                        if (!int.TryParse(s[1], out index)) return;

                        if (SelectedOption == null || SelectedOption != Options[index])
                        {
                            SelectedOption = Options[index];
                            OnToggled(new EventArgs());
                        }
                        else if (AllowEmptySelection)
                        {
                            SelectedOption = null;
                            OnToggled(new EventArgs());
                        }

                        RenderNecessary = true;

                        return;
                    }


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

            var lst = new List<ButtonBase>();
            foreach (var o in Options)
            {
                var index = Options.IndexOf(o);
                if (o == SelectedOption)
                {
                    lst.Add(new ButtonBase(SelectedIcon + " " + o.Text, "check$" + index));
                    continue;
                }

                lst.Add(new ButtonBase(o.Text, "check$" + index));
            }

            bf.AddButtonRow(lst);

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