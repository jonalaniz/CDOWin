using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CDOWin.ViewModels;

public partial class CalendarViewModel : ObservableObject {
    private readonly RemindersViewModel _remindersViewModel;

    public DateTime CurrentMonth { get; private set; } = DateTime.Today;
    public ObservableCollection<CalendarDay> Days { get; }

    public CalendarViewModel(RemindersViewModel viewModel) {
        _remindersViewModel = viewModel;
        Days = [];
    }

    public void BuildCalendarDays() {
        Days.Clear();

        // Get the first Sunday of the Calendar
        DateTime firstOfMonth = new(CurrentMonth.Year, CurrentMonth.Month, 1);
        int dayOfWeekOffset = (int)firstOfMonth.DayOfWeek;
        DateTime firstVisibleDay = firstOfMonth.AddDays(-dayOfWeekOffset);
        var reminders = _remindersViewModel.GetRemindersForMonth(firstOfMonth);

        for (int i = 0; i < 42; i++) {
            DateTime date = firstVisibleDay.AddDays(i);
            bool isCurrentMonth = date.Month == CurrentMonth.Month;
            var calendarDay = new CalendarDay(date, isCurrentMonth);
            if (isCurrentMonth)
                calendarDay.Reminders = FilterByDate(reminders, date);
            Days.Add(calendarDay);
        }
    }

    private ObservableCollection<Reminder> FilterByDate(ObservableCollection<Reminder> reminders, DateTime date) {
        var filteredReminders = new ObservableCollection<Reminder>();
        foreach (Reminder reminder in reminders) {
            if (reminder.date.Day == date.Day) {
                filteredReminders.Add(reminder);
            }
        }

        foreach(Reminder reminder in filteredReminders) {
            reminders.Remove(reminder);
        }

        return filteredReminders;
    }
}
