using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class RemindersViewModel : ObservableObject {
    private readonly IReminderService _service;
    private readonly ClientSelectionService _selectionService;

    [ObservableProperty]
    public partial ObservableCollection<Reminder> Reminders { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> FilteredReminders { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> ClientReminders { get; private set; } = [];

    [ObservableProperty]
    public partial Reminder? SelectedReminder { get; set; }

    public RemindersViewModel(IReminderService service, Services.ClientSelectionService clientSelectionService) {
        _service = service;
        _selectionService = clientSelectionService;
        _selectionService.SelectedClientChanged += OnClientChanged;
    }

    private void OnClientChanged(Client? client) {
        if (client != null && client.reminders != null) {
            List<Reminder> SortedReminders = client.reminders.OrderBy(o => o.date).ToList();
            ClientReminders.Clear();
            foreach (var reminder in SortedReminders) {
                ClientReminders.Add(reminder);
            }
        }
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
