using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotBase.Form
{
    /// <summary>
    ///     Base class for button handling
    /// </summary>
    public class TagButtonBase : ButtonBase
    {
        public TagButtonBase()
        {
        }

        public TagButtonBase(string Text, string Value, string Tag)
        {
            this.Text = Text;
            this.Value = Value;
            this.Tag = Tag;
        }

        public string Tag { get; set; }


        /// <summary>
        ///     Returns an inline Button
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public override InlineKeyboardButton ToInlineButton(ButtonForm form)
        {
            var id = form.DependencyControl != null ? form.DependencyControl.ControlID + "_" : "";

            return InlineKeyboardButton.WithCallbackData(Text, id + Value);
        }


        /// <summary>
        ///     Returns a KeyBoardButton
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public override KeyboardButton ToKeyboardButton(ButtonForm form)
        {
            return new KeyboardButton(Text);
        }
    }
}