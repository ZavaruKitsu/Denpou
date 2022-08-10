using TelegramBotBase.Attributes;

namespace TelegramBotBase.Form
{
    /// <summary>
    ///     A simple prompt dialog with one ok Button
    /// </summary>
    [IgnoreState]
    public class AlertDialog : ConfirmDialog
    {
        public AlertDialog(string Message, string ButtonText) : base(Message)
        {
            Buttons.Add(new ButtonBase(ButtonText, "ok"));
            this.ButtonText = ButtonText;
        }

        public string ButtonText { get; set; }
    }
}