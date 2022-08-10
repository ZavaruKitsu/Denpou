using System.Threading.Tasks;
using Denpou.Args;
using Denpou.Controls.Hybrid;
using Denpou.Enums;
using Denpou.Form;
using DenpouControls.Examples.Controls.Subclass;

namespace DenpouControls.Examples.Controls;

public class MultiViewForm : AutoCleanForm
{
    private ButtonGrid _bg;

    private MultiViewTest _mvt;

    public MultiViewForm()
    {
        DeleteMode = EDeleteMode.OnLeavingForm;
        Init += MultiViewForm_Init;
    }

    private Task MultiViewForm_Init(object sender, InitEventArgs e)
    {
        _mvt = new MultiViewTest();

        AddControl(_mvt);

        _bg = new ButtonGrid
        {
            DataSource = new ButtonForm()
        };
        _bg.DataSource.ButtonForm.AddButtonRow("Back", "$back$");
        _bg.ButtonClicked += Bg_ButtonClicked;
        _bg.KeyboardType = EKeyboardType.ReplyKeyboard;
        AddControl(_bg);
        return Task.CompletedTask;
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
