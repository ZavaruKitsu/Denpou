using System;
using System.Threading.Tasks;
using TelegramBotBase.Base;

namespace TelegramBotBase.Controls.Inline
{
    /// <summary>
    ///     A simple control for show and managing progress.
    /// </summary>
    public class ProgressBar : ControlBase
    {
        public enum eProgressStyle
        {
            standard = 0,
            squares = 1,
            circles = 2,
            lines = 3,
            squaredLines = 4,
            custom = 10
        }

        private eProgressStyle m_eStyle = eProgressStyle.standard;

        private int m_iMax = 100;

        private int m_iValue;

        public ProgressBar()
        {
            ProgressStyle = eProgressStyle.standard;

            Value = 0;
            Max = 100;

            RenderNecessary = true;
        }

        public ProgressBar(int Value, int Max, eProgressStyle Style)
        {
            this.Value = Value;
            this.Max = Max;
            ProgressStyle = Style;

            RenderNecessary = true;
        }

        public eProgressStyle ProgressStyle
        {
            get => m_eStyle;
            set
            {
                m_eStyle = value;
                LoadStyle();
            }
        }


        public int Value
        {
            get => m_iValue;
            set
            {
                if (value > Max) return;

                if (m_iValue != value) RenderNecessary = true;
                m_iValue = value;
            }
        }

        public int Max
        {
            get => m_iMax;
            set
            {
                if (m_iMax != value) RenderNecessary = true;
                m_iMax = value;
            }
        }

        public int? MessageId { get; set; }

        private bool RenderNecessary { get; set; }

        public int Steps
        {
            get
            {
                switch (ProgressStyle)
                {
                    case eProgressStyle.standard:

                        return 1;

                    case eProgressStyle.squares:

                        return 10;

                    case eProgressStyle.circles:

                        return 10;

                    case eProgressStyle.lines:

                        return 5;

                    case eProgressStyle.squaredLines:

                        return 5;

                    default:

                        return 1;
                }
            }
        }

        /// <summary>
        ///     Filled block (reached percentage)
        /// </summary>
        public string BlockChar { get; set; }

        /// <summary>
        ///     Unfilled block (not reached yet)
        /// </summary>
        public string EmptyBlockChar { get; set; }

        /// <summary>
        ///     String at the beginning of the progress bar
        /// </summary>
        public string StartChar { get; set; }

        /// <summary>
        ///     String at the end of the progress bar
        /// </summary>
        public string EndChar { get; set; }

        public override async Task Cleanup()
        {
            if (MessageId == null || MessageId == -1)
                return;


            await Device.DeleteMessage(MessageId.Value);
        }

        public void LoadStyle()
        {
            StartChar = "";
            EndChar = "";

            switch (ProgressStyle)
            {
                case eProgressStyle.circles:

                    BlockChar = "⚫️ ";
                    EmptyBlockChar = "⚪️ ";

                    break;
                case eProgressStyle.squares:

                    BlockChar = "⬛️ ";
                    EmptyBlockChar = "⬜️ ";

                    break;
                case eProgressStyle.lines:

                    BlockChar = "█";
                    EmptyBlockChar = "▁";

                    break;
                case eProgressStyle.squaredLines:

                    BlockChar = "▇";
                    EmptyBlockChar = "—";

                    StartChar = "[";
                    EndChar = "]";

                    break;
                case eProgressStyle.standard:
                case eProgressStyle.custom:

                    BlockChar = "";
                    EmptyBlockChar = "";

                    break;
            }
        }

        public override async Task Render(MessageResult result)
        {
            if (!RenderNecessary) return;

            if (Device == null) return;

            var message = "";
            var blocks = 0;
            var maxBlocks = 0;

            switch (ProgressStyle)
            {
                case eProgressStyle.standard:

                    message = Value.ToString("0") + "%";

                    break;

                case eProgressStyle.squares:
                case eProgressStyle.circles:
                case eProgressStyle.lines:
                case eProgressStyle.squaredLines:
                case eProgressStyle.custom:

                    blocks = (int)Math.Floor((decimal)Value / Steps);

                    maxBlocks = Max / Steps;

                    message += StartChar;

                    for (var i = 0; i < blocks; i++) message += BlockChar;

                    for (var i = 0; i < maxBlocks - blocks; i++) message += EmptyBlockChar;

                    message += EndChar;

                    message += " " + Value.ToString("0") + "%";

                    break;

                default:

                    return;
            }

            if (MessageId == null)
            {
                var m = await Device.Send(message);

                MessageId = m.MessageId;
            }
            else
            {
                await Device.Edit(MessageId.Value, message);
            }

            RenderNecessary = false;
        }
    }
}