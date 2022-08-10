using System;
using System.Threading.Tasks;
using Denpou.Args;
using Denpou.Controls.Inline;
using Denpou.Enums;
using Denpou.Form;

namespace DenpouControls.Examples.Controls;

public class ToggleButtons : AutoCleanForm
{
    public ToggleButtons()
    {
        DeleteMode = EDeleteMode.OnLeavingForm;

        Init += ToggleButtons_Init;
    }

    private Task ToggleButtons_Init(object sender, InitEventArgs e)
    {
        var tb = new ToggleButton
        {
            Checked = true
        };
        tb.Toggled += Tb_Toggled;

        AddControl(tb);

        tb = new ToggleButton
        {
            Checked = false
        };
        tb.Toggled += Tb_Toggled;

        AddControl(tb);

        tb = new ToggleButton
        {
            Checked = true
        };
        tb.Toggled += Tb_Toggled;

        AddControl(tb);
        return Task.CompletedTask;
    }

    private void Tb_Toggled(object sender, EventArgs e)
    {
        var tb = sender as ToggleButton;
        Console.WriteLine($"{tb.Id} was pressed, and toggled to {(tb.Checked ? "Checked" : "Unchecked")}");
    }
}