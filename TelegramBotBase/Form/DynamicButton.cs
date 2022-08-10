using System;

namespace TelegramBotBase.Form
{
    public class DynamicButton : ButtonBase
    {
        private readonly Func<string> GetText;

        private string m_text = "";

        public DynamicButton(string Text, string Value, string Url = null)
        {
            this.Text = Text;
            this.Value = Value;
            this.Url = Url;
        }

        public DynamicButton(Func<string> GetText, string Value, string Url = null)
        {
            this.GetText = GetText;
            this.Value = Value;
            this.Url = Url;
        }

        public override string Text
        {
            get => GetText?.Invoke() ?? m_text;
            set => m_text = value;
        }
    }
}