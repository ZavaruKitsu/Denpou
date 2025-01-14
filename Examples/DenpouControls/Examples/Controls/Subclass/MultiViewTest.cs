﻿using System.Threading.Tasks;
using Denpou.Args;
using Denpou.Base;
using Denpou.Controls.Hybrid;
using Denpou.Form;

namespace DenpouControls.Examples.Controls.Subclass;

public class MultiViewTest : MultiView
{
    public override Task Action(MessageResult result, string value = null)
    {
        switch (result.RawData)
        {
            case "back":

                SelectedViewIndex--;

                break;
            case "next":

                SelectedViewIndex++;

                break;
        }

        return Task.CompletedTask;
    }

    public override async Task RenderView(RenderViewEventArgs e)
    {
        var bf = new ButtonForm();
        bf.AddButtonRow(new ButtonBase("Back", "back"), new ButtonBase("Next", "next"));

        switch (e.CurrentView)
        {
            case 0:
            case 1:
            case 2:

                await Device.Send($"Page {e.CurrentView + 1}", bf);

                break;

            default:

                await Device.Send("Unknown Page", bf);

                break;
        }
    }
}