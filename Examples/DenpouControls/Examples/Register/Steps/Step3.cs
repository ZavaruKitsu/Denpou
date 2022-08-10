using System.Threading.Tasks;
using Denpou.Base;
using Denpou.Form;

namespace DenpouControls.Examples.Register.Steps;

public class Step3 : AutoCleanForm
{
    public Data UserData { get; set; }

    public override Task Load(MessageResult message)
    {
        if (message.Handled)
            return Task.CompletedTask;

        if (message.MessageText.Trim() == "")
            return Task.CompletedTask;

        if (UserData.EMail == null) UserData.EMail = message.MessageText;

        return Task.CompletedTask;
    }

    public override async Task Action(MessageResult message)
    {
        await message.ConfirmAction();

        switch (message.RawData)
        {
            case "back":

                var start = new Start();

                await NavigateTo(start);

                break;
        }
    }

    public override async Task Render(MessageResult message)
    {
        if (UserData.EMail == null)
        {
            await Device.Send("Please sent your email:");
            return;
        }

        message.Handled = true;

        var s = "";

        s += $"Firstname: {UserData.Firstname}\n";
        s += $"Lastname: {UserData.Lastname}\n";
        s += $"E-Mail: {UserData.EMail}\n";

        var bf = new ButtonForm();
        bf.AddButtonRow(new ButtonBase("Back", "back"));

        await Device.Send($"Your details:\n{s}", bf);
    }
}