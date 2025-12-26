using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.Views.Reminders;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class RemindersViewModel : ObservableObject {
    private readonly IReminderService _service;
    private readonly ClientSelectionService _selectionService;

    private RemindersFilter _filter = RemindersFilter.All;
    public RemindersFilter Filter {
        get => _filter;
        set {
            _filter = value;
            ApplyFilter();
        }
    }

    [ObservableProperty]
    public partial ObservableCollection<Reminder> All { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> ClientSpecific { get; private set; } = [];

    [ObservableProperty]
    public partial Reminder? SelectedReminder { get; set; }

    [ObservableProperty]
    public string endText = "";

    public RemindersViewModel(IReminderService service, Services.ClientSelectionService clientSelectionService) {
        _service = service;
        _selectionService = clientSelectionService;
        _selectionService.SelectedClientChanged += OnClientChanged;
    }

    private void OnClientChanged(Client? client) {
        ClientSpecific.Clear();
        if (client?.reminders != null) {
            foreach (var reminder in client.reminders.OrderBy(o => o.date)) {
                ClientSpecific.Add(reminder);
            }

            if (Filter == RemindersFilter.Client) {
                ReplaceFiltered(ClientSpecific);
                UpdateEndText();
            }
        }
    }

    partial void OnSelectedReminderChanged(Reminder? value) {
        if (value != null)
            _ = RefreshSelectedReminderAsync(value.id);
    }

    // Public Methods
    public void RequestClient(int clientID) {
        _selectionService.RequestSelectedClient(clientID);
    }

    public void ToggleCompleted(int id) {
        var reminder = Filtered.First(r => r.id == id);
        if (reminder != null) {
            var update = new UpdateReminderDTO();
            update.complete = !reminder.complete;
            _ = UpdateReminder(id, update);
        }
    }

    private void ApplyFilter() {
        switch (Filter) {
            case RemindersFilter.All:
                ReplaceFiltered(All);
                break;
            case RemindersFilter.Upcoming:
                SetUpcomingReminders();
                break;
            case RemindersFilter.Client:
                ReplaceFiltered(ClientSpecific);
                break;
            default:
                break;
        }
        UpdateEndText();
    }

    public void ApplyDateFilter(DateTime date) {
        Filter = RemindersFilter.Date;
        Filtered.Clear();
        foreach (var reminder in All) {
            if (reminder.date.Date == date.Date)
                Filtered.Add(reminder);
        }
        UpdateEndText();
    }

    public Reminder GetReminderByID(int id) {
        return Filtered.FirstOrDefault(r => r.id == id);
    }

    // Utility Methods

    private void ReplaceFiltered(IEnumerable<Reminder> source) {
        Filtered.Clear();
        foreach (var reminder in source)
            Filtered.Add(reminder);
    }

    private void SetUpcomingReminders() {
        Filtered.Clear();
        foreach (var reminder in All) {
            if (reminder.date > DateTime.Now)
                Filtered.Add(reminder);
        }
    }

    private void UpdateEndText() {
        if (Filtered.Count == 0) {
            EndText = "There are no reminders 静か";
        } else {
            EndText = "We have reached the end of the list 和";
        }
    }

    // CRUD Methods
    public async Task LoadRemindersAsync() {
        var reminders = await _service.GetAllRemindersAsync();

        All.Clear();
        foreach (var reminder in reminders.OrderBy(o => o.date)) {
            All.Add(reminder);
        }
    }

    public async Task RefreshSelectedReminderAsync(int id) {
        var reminder = await _service.GetReminderAsync(id);
        if (SelectedReminder != reminder) {
            SelectedReminder = reminder;

            var index = All.IndexOf(All.First(r => r.id == id));
            All[index] = reminder;
        }
    }

    public async Task UpdateReminder(int id, UpdateReminderDTO update) {
        var updatedReminder = await _service.UpdateReminderAsync(id, update);
        Replace(All, updatedReminder);
        Replace(Filtered, updatedReminder);
        Replace(ClientSpecific, updatedReminder);
    }

    private void Replace(ObservableCollection<Reminder> list, Reminder updated) {
        var index = list.IndexOf(list.First(r => r.id == updated.id));
        if (index >= 0)
            list[index] = updated;
    }
}
