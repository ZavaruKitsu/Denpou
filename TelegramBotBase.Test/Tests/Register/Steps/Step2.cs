using System.Threading.Tasks;
using TelegramBotBase.Base;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Register.Steps
{
    public class Step2 : AutoCleanForm
    {
        public Data UserData { get; set; }


        public override async Task Load(MessageResult message)
        {
            if (message.Handled)
                return;

            if (message.MessageText.Trim() == "")
                return;

            if (UserData.Lastname == null)
            {
                UserData.Lastname = message.MessageText;
            }
        }


        public override async Task Render(MessageResult message)
        {
            if (UserData.Lastname == null)
            {
                await Device.Send("Please sent your lastname:");
                return;
            }

            message.Handled = true;

            var step3 = new Step3();

            step3.UserData = UserData;

            await NavigateTo(step3);
        }
    }
}