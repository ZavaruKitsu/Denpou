using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Controls.Hybrid;
using TelegramBotBase.Enums;
using TelegramBotBase.Form;
using TelegramBotBaseTest.Tests.Controls.Subclass;

namespace TelegramBotBaseTest.Tests.Controls
{
    public class MultiViewForm : AutoCleanForm
    {
        private ButtonGrid bg;

        private MultiViewTest mvt;

        public MultiViewForm()
        {
            DeleteMode = eDeleteMode.OnLeavingForm;
            Init += MultiViewForm_Init;
        }

        private async Task MultiViewForm_Init(object sender, InitEventArgs e)
        {
            mvt = new MultiViewTest();

            AddControl(mvt);

            bg = new ButtonGrid();
            bg.ButtonsForm = new ButtonForm();
            bg.ButtonsForm.AddButtonRow("Back", "$back$");
            bg.ButtonClicked += Bg_ButtonClicked;
            bg.KeyboardType = eKeyboardType.ReplyKeyboard;
            AddControl(bg);
        }

        private async Task Bg_ButtonClicked(object sender, ButtonClickedEventArgs e)
        {
            switch (e.Button.Value)
            {
                case "$back$":

                    var mn = new Menu();
                    await NavigateTo(mn);

                    break;
            }
        }
    }
}