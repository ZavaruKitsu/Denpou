using System.Linq;
using Telegram.Bot.Types.Enums;

namespace TelegramBotBase.Markdown
{
    /// <summary>
    ///     https://core.telegram.org/bots/api#markdownv2-style
    /// </summary>
    public static class Generator
    {
        public static ParseMode OutputMode { get; set; } = ParseMode.Markdown;

        /// <summary>
        ///     Generates a link with title in Markdown or HTML
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        public static string Link(this string url, string title = null, string tooltip = null)
        {
            switch (OutputMode)
            {
                case ParseMode.Markdown:
                    return "[" + (title ?? url) + "](" + url + " " + (tooltip ?? "") + ")";
                case ParseMode.Html:
                    return $"<a href=\"{url}\" title=\"{tooltip ?? ""}\">{title ?? ""}</b>";
            }

            return url;
        }

        /// <summary>
        ///     Returns a Link to the User, title is optional.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string MentionUser(this int userId, string title = null)
        {
            return Link("tg://user?id=" + userId, title);
        }

        /// <summary>
        ///     Returns a Link to the User, title is optional.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string MentionUser(this string username, string title = null)
        {
            return Link("tg://user?id=" + username, title);
        }

        /// <summary>
        ///     Returns a bold text in Markdown or HTML
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Bold(this string text)
        {
            switch (OutputMode)
            {
                case ParseMode.Markdown:
                    return "*" + text + "*";
                case ParseMode.Html:
                    return "<b>" + text + "</b>";
            }

            return text;
        }

        /// <summary>
        ///     Returns a strike through in Markdown or HTML
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Strikesthrough(this string text)
        {
            switch (OutputMode)
            {
                case ParseMode.Markdown:
                    return "~" + text + "~";
                case ParseMode.Html:
                    return "<s>" + text + "</s>";
            }

            return text;
        }

        /// <summary>
        ///     Returns a italic text in Markdown or HTML
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Italic(this string text)
        {
            switch (OutputMode)
            {
                case ParseMode.Markdown:
                    return "_" + text + "_";
                case ParseMode.Html:
                    return "<i>" + text + "</i>";
            }

            return text;
        }

        /// <summary>
        ///     Returns a underline text in Markdown or HTML
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Underline(this string text)
        {
            switch (OutputMode)
            {
                case ParseMode.Markdown:
                    return "__" + text + "__";
                case ParseMode.Html:
                    return "<u>" + text + "</u>";
            }

            return text;
        }

        /// <summary>
        ///     Returns a monospace text in Markdown or HTML
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Monospace(this string text)
        {
            switch (OutputMode)
            {
                case ParseMode.Markdown:
                    return "`" + text + "`";
                case ParseMode.Html:
                    return "<code>" + text + "</code>";
            }

            return text;
        }

        /// <summary>
        ///     Returns a multi monospace text in Markdown or HTML
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MultiMonospace(this string text)
        {
            switch (OutputMode)
            {
                case ParseMode.Markdown:
                    return "```" + text + "```";
                case ParseMode.Html:
                    return "<pre>" + text + "</pre>";
            }

            return text;
        }

        /// <summary>
        ///     Escapes all characters as stated in the documentation: https://core.telegram.org/bots/api#markdownv2-style
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MarkdownV2Escape(this string text, params char[] toKeep)
        {
            char[] toEscape =
                { '_', '*', '[', ']', '(', ')', '~', '`', '>', '#', '+', '-', '=', '|', '{', '}', '.', '!' };

            return text.EscapeAll(toEscape.Where(a => !toKeep.Contains(a)).Select(a => a.ToString()).ToArray());
        }

        public static string EscapeAll(this string seed, string[] chars, char escapeCharacter = '\\')
        {
            return chars.Aggregate(seed, (str, cItem) => str.Replace(cItem, escapeCharacter + cItem));
        }
    }
}
