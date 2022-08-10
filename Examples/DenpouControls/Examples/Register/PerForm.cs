using System.Threading.Tasks;
using Denpou.Base;
using Denpou.Form;

namespace DenpouControls.Examples.Register;

public class PerForm : AutoCleanForm
{
    public string EMail { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public override Task Load(MessageResult message)
    {
        if (message.MessageText.Trim() == "")
            return Task.CompletedTask;

        if (Firstname == null)
        {
            Firstname = message.MessageText;
            return Task.CompletedTask;
        }

        if (Lastname == null)
        {
            Lastname = message.MessageText;
            return Task.CompletedTask;
        }

        if (EMail == null) EMail = message.MessageText;

        return Task.CompletedTask;
    }

    public override async Task Action(MessageResult message)
    {
        var call = message.GetData<CallbackData>();

        await message.ConfirmAction();

        if (call == null)
            return;

        switch (call.Value)
        {
            case "back":

                var start = new Start();

                await NavigateTo(start);

                break;
        }
    }

    public override async Task Render(MessageResult message)
    {
        if (Firstname == null)
        {
            await Device.Send("Please sent your firstname:");
            return;
        }

        if (Lastname == null)
        {
            await Device.Send("Please sent your lastname:");
            return;
        }

        if (EMail == null)
        {
            await Device.Send("Please sent your email address:");
            return;
        }


        var s = "";

        s += $"Firstname: {Firstname}\n";
        s += $"Lastname: {Lastname}\n";
        s += $"E-Mail: {EMail}\n";

        var bf = new ButtonForm();
        bf.AddButtonRow(new ButtonBase("Back", new CallbackData("a", "back").Serialize()));

        await Device.Send($"Your details:\n{s}", bf);
    }
}