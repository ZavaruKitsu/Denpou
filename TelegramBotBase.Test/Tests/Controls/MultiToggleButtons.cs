﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Controls.Inline;
using TelegramBotBase.Enums;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Controls
{
    public class MultiToggleButtons : AutoCleanForm
    {
        public MultiToggleButtons()
        {
            DeleteMode = eDeleteMode.OnLeavingForm;

            Init += ToggleButtons_Init;
        }

        private async Task ToggleButtons_Init(object sender, InitEventArgs e)
        {
            var mtb = new MultiToggleButton();

            mtb.Options = new List<ButtonBase>
                { new ButtonBase("Option 1", "1"), new ButtonBase("Option 2", "2"), new ButtonBase("Option 3", "3") };
            mtb.SelectedOption = mtb.Options.FirstOrDefault();
            mtb.Toggled += Tb_Toggled;
            AddControl(mtb);

            mtb = new MultiToggleButton();

            mtb.Options = new List<ButtonBase>
                { new ButtonBase("Option 4", "4"), new ButtonBase("Option 5", "5"), new ButtonBase("Option 6", "6") };
            mtb.SelectedOption = mtb.Options.FirstOrDefault();
            mtb.AllowEmptySelection = false;
            mtb.Toggled += Tb_Toggled;
            AddControl(mtb);
        }

        private void Tb_Toggled(object sender, EventArgs e)
        {
            var tb = sender as MultiToggleButton;
            if (tb.SelectedOption != null)
            {
                Console.WriteLine(tb.ID + " was pressed, and toggled to " + tb.SelectedOption.Value);
                return;
            }

            Console.WriteLine("Selection for " + tb.ID + " has been removed.");
        }
    }
}