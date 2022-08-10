using Denpou.Enums;

namespace Denpou.Controls.Inline;

public class MonthPicker : CalendarPicker
{
    public MonthPicker()
    {
        PickerMode = EMonthPickerMode.Month;
        EnableDayView = false;
    }
}