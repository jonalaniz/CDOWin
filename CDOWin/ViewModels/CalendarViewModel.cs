using CDO.Core.DTOs.Reminders;
using CDO.Core.Models;
using CDOWin.Composers;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CalendarViewModel(RemindersViewModel viewModel) : ObservableObject {
    private readonly RemindersViewModel _remindersViewModel = viewModel;

    public DateTime CurrentMonth { get; private set; } = DateTime.Today;
    public ObservableCollection<CalendarDay> Days { get; } = [];

    public void BuildCalendarDays() {
        Days.Clear();

        var firstOfMonth = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
        var firstVisibleDay = firstOfMonth.AddDays(-(int)firstOfMonth.DayOfWeek);

        var reminders = _remindersViewModel.GetRemindersByMonth(firstOfMonth);

        for (int i = 0; i < 42; i++) {
            var date = firstVisibleDay.AddDays(i);

            Days.Add(new CalendarDay(date, date.Month == CurrentMonth.Month) {
                Reminders = new ObservableCollection<Reminder>(
                    reminders.GetValueOrDefault(date.Date) ?? []
                    )
            });
        }
    }

    public void ExportReminders() {
        var list = _remindersViewModel.GetRemindsListForMonth(CurrentMonth);
        var composer = new RemindersComposer();
        composer.BuildCSV(list);
    }

    public void SetCurrentMonth() {
        if (CurrentMonth.Month == DateTime.Now.Month) return;
        CurrentMonth = DateTime.Now;
        BuildCalendarDays();
    }

    public void IncrementMonth() {
        CurrentMonth = CurrentMonth.AddMonths(1);
        BuildCalendarDays();
    }

    public void DecrementMonth() {
        CurrentMonth = CurrentMonth.AddMonths(-1);
        BuildCalendarDays();
    }

    public Reminder? GetReminderByID(int id) => _remindersViewModel.GetReminderByID(id);

    public async Task UpdateReminderAsync(int id, ReminderUpdate update) {
        await _remindersViewModel.UpdateReminderAsync(id, update);
        BuildCalendarDays();
        // the task above both updates and reloads the data, we need to refresh the 
    }
}
