using System;
using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Controls.Inline;
using TelegramBotBase.Enums;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Controls
{
    public class ToggleButtons : AutoCleanForm
    {
        public ToggleButtons()
        {
            DeleteMode = eDeleteMode.OnLeavingForm;

            Init += ToggleButtons_Init;
        }

        private async Task ToggleButtons_Init(object sender, InitEventArgs e)
        {
            var tb = new ToggleButton();
            tb.Checked = true;
            tb.Toggled += Tb_Toggled;

            AddControl(tb);

            tb = new ToggleButton();
            tb.Checked = false;
            tb.Toggled += Tb_Toggled;

            AddControl(tb);

            tb = new ToggleButton();
            tb.Checked = true;
            tb.Toggled += Tb_Toggled;

            AddControl(tb);
        }

        private void Tb_Toggled(object sender, EventArgs e)
        {
            var tb = sender as ToggleButton;
            Console.WriteLine(tb.ID + " was pressed, and toggled to " + (tb.Checked ? "Checked" : "Unchecked"));
        }
    }
}