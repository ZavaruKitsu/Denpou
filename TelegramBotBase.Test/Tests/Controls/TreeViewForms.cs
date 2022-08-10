﻿using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Base;
using TelegramBotBase.Controls.Inline;
using TelegramBotBase.Enums;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Controls
{
    public class TreeViewForms : AutoCleanForm
    {
        public TreeViewForms()
        {
            DeleteMode = eDeleteMode.OnLeavingForm;
            Init += TreeViewForms_Init;
        }

        public TreeView view { get; set; }

        private int? MessageId { get; set; }

        private async Task TreeViewForms_Init(object sender, InitEventArgs e)
        {
            view = new TreeView();

            var tvn = new TreeViewNode("Cars", "cars");

            tvn.AddNode(new TreeViewNode("Porsche", "porsche",
                new TreeViewNode("Website", "web", "https://www.porsche.com/germany/"), new TreeViewNode("911", "911"),
                new TreeViewNode("918 Spyder", "918")));
            tvn.AddNode(new TreeViewNode("BMW", "bmw"));
            tvn.AddNode(new TreeViewNode("Audi", "audi"));
            tvn.AddNode(new TreeViewNode("VW", "vw"));
            tvn.AddNode(new TreeViewNode("Lamborghini", "lamborghini"));

            view.Nodes.Add(tvn);

            tvn = new TreeViewNode("Fruits", "fruits");

            tvn.AddNode(new TreeViewNode("Apple", "apple"));
            tvn.AddNode(new TreeViewNode("Orange", "orange"));
            tvn.AddNode(new TreeViewNode("Lemon", "lemon"));

            view.Nodes.Add(tvn);

            AddControl(view);
        }

        public override async Task Action(MessageResult message)
        {
            await message.ConfirmAction();

            if (message.Handled)
                return;

            switch (message.RawData)
            {
                case "back":

                    message.Handled = true;

                    var start = new Menu();

                    await NavigateTo(start);

                    break;
            }
        }

        public override async Task Render(MessageResult message)
        {
            var s = "";

            s += "Selected Node: " + (view.SelectedNode?.Text ?? "(null)") + "\r\n";

            s += "Visible Node: " + (view.VisibleNode?.Text ?? "(top)") + "\r\n";

            s += "Visible Path: " + view.GetPath() + "\r\n";
            s += "Selected Path: " + (view.SelectedNode?.GetPath() ?? "(null)") + "\r\n";

            var bf = new ButtonForm();
            bf.AddButtonRow(new ButtonBase("Back", "back"));

            if (MessageId != null)
            {
                await Device.Edit(MessageId.Value, s, bf);
            }
            else
            {
                var m = await Device.Send(s, bf);
                MessageId = m.MessageId;
            }
        }
    }
}