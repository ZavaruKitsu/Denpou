using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Controls.Hybrid;
using TelegramBotBase.Enums;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Controls
{
    public class ButtonGridForm : AutoCleanForm
    {
        private ButtonGrid m_Buttons;

        public ButtonGridForm()
        {
            DeleteMode = eDeleteMode.OnLeavingForm;

            Init += ButtonGridForm_Init;
        }

        private async Task ButtonGridForm_Init(object sender, InitEventArgs e)
        {
            m_Buttons = new ButtonGrid();

            m_Buttons.KeyboardType = eKeyboardType.InlineKeyBoard;

            var bf = new ButtonForm();

            bf.AddButtonRow(new ButtonBase("Back", "back"), new ButtonBase("Switch Keyboard", "switch"));

            bf.AddButtonRow(new ButtonBase("Button1", "b1"), new ButtonBase("Button2", "b2"));

            bf.AddButtonRow(new ButtonBase("Button3", "b3"), new ButtonBase("Button4", "b4"));

            m_Buttons.ButtonsForm = bf;

            m_Buttons.ButtonClicked += Bg_ButtonClicked;

            AddControl(m_Buttons);
        }

        private async Task Bg_ButtonClicked(object sender, ButtonClickedEventArgs e)
        {
            if (e.Button == null)
                return;

            if (e.Button.Value == "back")
            {
                var start = new Menu();
                await NavigateTo(start);
            }
            else if (e.Button.Value == "switch")
            {
                switch (m_Buttons.KeyboardType)
                {
                    case eKeyboardType.ReplyKeyboard:
                        m_Buttons.KeyboardType = eKeyboardType.InlineKeyBoard;
                        break;
                    case eKeyboardType.InlineKeyBoard:
                        m_Buttons.KeyboardType = eKeyboardType.ReplyKeyboard;
                        break;
                }
            }
            else
            {
                await Device.Send($"Button clicked with Text: {e.Button.Text} and Value {e.Button.Value}");
            }
        }
    }
}