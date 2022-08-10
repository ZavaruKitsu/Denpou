using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Base;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Register.Steps
{
    public class Step1 : AutoCleanForm
    {
        public Step1()
        {
            Init += Step1_Init;
        }

        public Data UserData { get; set; }

        private async Task Step1_Init(object sender, InitEventArgs e)
        {
            UserData = new Data();
        }


        public override async Task Load(MessageResult message)
        {
            if (message.Handled)
                return;

            if (message.MessageText.Trim() == "")
                return;

            if (UserData.Firstname == null)
            {
                UserData.Firstname = message.MessageText;
            }
        }

        public override async Task Render(MessageResult message)
        {
            if (UserData.Firstname == null)
            {
                await Device.Send("Please sent your firstname:");
                return;
            }

            message.Handled = true;

            var step2 = new Step2();

            step2.UserData = UserData;

            await NavigateTo(step2);
        }
    }
}