using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class RemindersViewModel : ObservableObject {
    private readonly IReminderService _service;

    [ObservableProperty]
    public partial ObservableCollection<Reminder> Reminders { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> FilteredReminders { get; private set; } = [];

    [ObservableProperty]
    public partial Reminder? SelectedReminder { get; set; }

    public RemindersViewModel(IReminderService service) {
        _service = service;
    }

    partial void OnSelectedReminderChanged(Reminder? value) {
        if (value != null)
            _ = RefreshSelectedReminder(value.id);
    }

    public async Task LoadRemindersAsync() {
        var reminders = await _service.GetAllRemindersAsync();

        List<Reminder> SortedReminders = reminders.OrderBy(o => o.date).ToList();
        Reminders.Clear();

        foreach (var reminder in SortedReminders) {
            Reminders.Add(reminder);
        }
    }

    public async Task RefreshSelectedReminder(int id) {
        var reminder = await _service.GetReminderAsync(id);
        if (SelectedReminder != reminder) {
            SelectedReminder = reminder;

            var index = Reminders.IndexOf(Reminders.First(r => r.id == id));
            Reminders[index] = reminder;
        }
    }
}
