using CDO.Core.DTOs.Reminders;
using System.Collections.ObjectModel;

namespace CDO.Core.Models;

public class CalendarDay {
    public DateTime Date { get; }
    public bool IsCurrentMonth { get; }

    public ObservableCollection<ReminderDetail> Reminders { get; set; }

    public CalendarDay(DateTime date, bool isCurrentMonth) {
        Date = date;
        IsCurrentMonth = isCurrentMonth;
        Reminders = [];
    }

}
