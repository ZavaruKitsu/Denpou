using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Base;
using TelegramBotBase.Controls.Inline;
using TelegramBotBase.Enums;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Controls
{
    public class CalendarPickerForm : AutoCleanForm
    {
        public CalendarPickerForm()
        {
            DeleteMode = eDeleteMode.OnLeavingForm;
            Init += CalendarPickerForm_Init;
        }

        public CalendarPicker Picker { get; set; }

        private int? selectedDateMessage { get; set; }

        private async Task CalendarPickerForm_Init(object sender, InitEventArgs e)
        {
            Picker = new CalendarPicker();
            Picker.Title = "Datum auswählen / Pick date";

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

            s = "Selected date is " + Picker.SelectedDate.ToShortDateString() + "\r\n";
            s += "Selected month is " + Picker.Culture.DateTimeFormat.MonthNames[Picker.VisibleMonth.Month - 1] +
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