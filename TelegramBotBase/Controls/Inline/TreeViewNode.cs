using System.Collections.Generic;
using System.Linq;

namespace TelegramBotBase.Controls.Inline
{
    public class TreeViewNode
    {
        public TreeViewNode(string Text, string Value)
        {
            this.Text = Text;
            this.Value = Value;
        }

        public TreeViewNode(string Text, string Value, string Url) : this(Text, Value)
        {
            this.Url = Url;
        }

        public TreeViewNode(string Text, string Value, params TreeViewNode[] childnodes) : this(Text, Value)
        {
            foreach (var c in childnodes) AddNode(c);
        }

        public string Text { get; set; }

        public string Value { get; set; }

        public string Url { get; set; }

        public List<TreeViewNode> ChildNodes { get; set; } = new List<TreeViewNode>();

        public TreeViewNode ParentNode { get; set; }


        public void AddNode(TreeViewNode node)
        {
            node.ParentNode = this;
            ChildNodes.Add(node);
        }

        public TreeViewNode FindNodeByValue(string Value)
        {
            return ChildNodes.FirstOrDefault(a => a.Value == Value);
        }

        public string GetPath()
        {
            var s = "\\" + Value;
            var p = this;
            while (p.ParentNode != null)
            {
                s = "\\" + p.ParentNode.Value + s;
                p = p.ParentNode;
            }

            return s;
        }
    }
}