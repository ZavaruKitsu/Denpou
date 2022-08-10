using System;
using System.Threading.Tasks;
using Denpou.Base;

namespace Denpou.Controls.Inline;

/// <summary>
///     A simple control for show and managing progress.
/// </summary>
public class ProgressBar : ControlBase
{
    public enum EProgressStyle
    {
        Standard = 0,
        Squares = 1,
        Circles = 2,
        Lines = 3,
        SquaredLines = 4,
        Custom = 10
    }

    private EProgressStyle _mEStyle = EProgressStyle.Standard;

    private int _mIMax = 100;

    private int _mIValue;

    public ProgressBar()
    {
        ProgressStyle = EProgressStyle.Standard;

        Value = 0;
        Max = 100;

        RenderNecessary = true;
    }

    public ProgressBar(int value, int max, EProgressStyle style)
    {
        Value = value;
        Max = max;
        ProgressStyle = style;

        RenderNecessary = true;
    }

    public EProgressStyle ProgressStyle
    {
        get => _mEStyle;
        set
        {
            _mEStyle = value;
            LoadStyle();
        }
    }


    public int Value
    {
        get => _mIValue;
        set
        {
            if (value > Max) return;

            if (_mIValue != value) RenderNecessary = true;
            _mIValue = value;
        }
    }

    public int Max
    {
        get => _mIMax;
        set
        {
            if (_mIMax != value) RenderNecessary = true;
            _mIMax = value;
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
                case EProgressStyle.Standard:

                    return 1;

                case EProgressStyle.Squares:

                    return 10;

                case EProgressStyle.Circles:

                    return 10;

                case EProgressStyle.Lines:

                    return 5;

                case EProgressStyle.SquaredLines:

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
        if (MessageId is null or -1)
            return;


        await Device.DeleteMessage(MessageId.Value);
    }

    public void LoadStyle()
    {
        StartChar = "";
        EndChar = "";

        switch (ProgressStyle)
        {
            case EProgressStyle.Circles:

                BlockChar = "⚫️ ";
                EmptyBlockChar = "⚪️ ";

                break;
            case EProgressStyle.Squares:

                BlockChar = "⬛️ ";
                EmptyBlockChar = "⬜️ ";

                break;
            case EProgressStyle.Lines:

                BlockChar = "█";
                EmptyBlockChar = "▁";

                break;
            case EProgressStyle.SquaredLines:

                BlockChar = "▇";
                EmptyBlockChar = "—";

                StartChar = "[";
                EndChar = "]";

                break;
            case EProgressStyle.Standard:
            case EProgressStyle.Custom:

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
            case EProgressStyle.Standard:

                message = $"{Value:0}%";

                break;

            case EProgressStyle.Squares:
            case EProgressStyle.Circles:
            case EProgressStyle.Lines:
            case EProgressStyle.SquaredLines:
            case EProgressStyle.Custom:

                blocks = (int)Math.Floor((decimal)Value / Steps);

                maxBlocks = Max / Steps;

                message += StartChar;

                for (var i = 0; i < blocks; i++) message += BlockChar;

                for (var i = 0; i < maxBlocks - blocks; i++) message += EmptyBlockChar;

                message += EndChar;

                message += $" {Value:0}%";

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