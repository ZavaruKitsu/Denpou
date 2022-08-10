﻿using System.Threading.Tasks;
using Denpou.Args;
using Denpou.Base;
using Denpou.Controls.Inline;
using Denpou.Enums;
using Denpou.Form;

namespace DenpouControls.Examples.Controls;

public class MonthPickerForm : AutoCleanForm
{
    public MonthPickerForm()
    {
        DeleteMode = EDeleteMode.OnLeavingForm;
        Init += MonthPickerForm_Init;
    }

    public MonthPicker Picker { get; set; }

    private int? SelectedDateMessage { get; set; }

    private Task MonthPickerForm_Init(object sender, InitEventArgs e)
    {
        Picker = new MonthPicker
        {
            Title = "Monat auswählen / Pick month"
        };
        AddControl(Picker);
        return Task.CompletedTask;
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

        s += $"Selected month is {Picker.Culture.DateTimeFormat.MonthNames[Picker.SelectedDate.Month - 1]}\n";
        s += $"Selected year is {Picker.VisibleMonth.Year}";

        var bf = new ButtonForm();
        bf.AddButtonRow(new ButtonBase("Back", "back"));

        if (SelectedDateMessage != null)
        {
            await Device.Edit(SelectedDateMessage.Value, s, bf);
        }
        else
        {
            var m = await Device.Send(s, bf);
            SelectedDateMessage = m.MessageId;
        }
    }
}