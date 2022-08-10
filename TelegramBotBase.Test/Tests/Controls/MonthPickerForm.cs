using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Base;
using TelegramBotBase.Controls.Inline;
using TelegramBotBase.Enums;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Controls
{
    public class MonthPickerForm : AutoCleanForm
    {
        public MonthPickerForm()
        {
            DeleteMode = eDeleteMode.OnLeavingForm;
            Init += MonthPickerForm_Init;
        }

        public MonthPicker Picker { get; set; }

        private int? selectedDateMessage { get; set; }

        private async Task MonthPickerForm_Init(object sender, InitEventArgs e)
        {
            Picker = new MonthPicker();
            Picker.Title = "Monat auswählen / Pick month";
            AddControl(Picker);
        }


        public override async Task Action(MessageResult message)
        {
            switch (message.RawData)
            {
                case "back":

                    var s = new Menu();

                    await NavigateTo(s);

                    break;
            }
        }

        public override async Task Render(MessageResult message)
        {
            var s = "";

            s += "Selected month is " + Picker.Culture.DateTimeFormat.MonthNames[Picker.SelectedDate.Month - 1] +
                 "\r\n";
            s += "Selected year is " + Picker.VisibleMonth.Year;

            var bf = new ButtonForm();
            bf.AddButtonRow(new ButtonBase("Back", "back"));

            if (selectedDateMessage != null)
            {
                await Device.Edit(selectedDateMessage.Value, s, bf);
            }
            else
            {
                var m = await Device.Send(s, bf);
                selectedDateMessage = m.MessageId;
            }
        }
    }
}