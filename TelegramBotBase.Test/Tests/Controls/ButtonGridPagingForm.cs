using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Controls.Hybrid;
using TelegramBotBase.Enums;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Controls
{
    public class ButtonGridPagingForm : AutoCleanForm
    {
        private ButtonGrid m_Buttons;

        public ButtonGridPagingForm()
        {
            DeleteMode = eDeleteMode.OnLeavingForm;

            Init += ButtonGridForm_Init;
        }

        private async Task ButtonGridForm_Init(object sender, InitEventArgs e)
        {
            m_Buttons = new ButtonGrid();

            m_Buttons.KeyboardType = eKeyboardType.ReplyKeyboard;

            m_Buttons.EnablePaging = true;
            m_Buttons.EnableSearch = true;

            m_Buttons.HeadLayoutButtonRow = new List<ButtonBase> { new ButtonBase("Back", "back") };

            var countries = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var bf = new ButtonForm();

            foreach (var c in countries) bf.AddButtonRow(new ButtonBase(c.EnglishName, c.EnglishName));

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
            else
            {
                await Device.Send($"Button clicked with Text: {e.Button.Text} and Value {e.Button.Value}");
            }
        }
    }
}