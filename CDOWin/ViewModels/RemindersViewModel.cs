using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.Views.Clients.Dialogs;
using CDOWin.Views.Reminders;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class RemindersViewModel : ObservableObject {
    private readonly IReminderService _service;
    private readonly ClientSelectionService _selectionService;

    public RemindersFilter Filter {
        get => _filter;
        set {
            _filter = value;
            ApplyFilter();
        }
    }

    private RemindersFilter _filter = RemindersFilter.All;

    [ObservableProperty]
    public partial ObservableCollection<Reminder> All { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> ClientSpecific { get; private set; } = [];


    [ObservableProperty]
    public partial string EndText { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public RemindersViewModel(IReminderService service, Services.ClientSelectionService clientSelectionService) {
        _service = service;
        _selectionService = clientSelectionService;
        _selectionService.SelectedClientChanged += OnClientChanged;
        _selectionService.NewReminderCreated += OnReminderCreated;
    }

    // =========================
    // Property Change Methods
    // =========================
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

    private void OnReminderCreated() {
        Debug.WriteLine("New Reminder Created");
        _ = LoadRemindersAsync();
    }

    // =========================
    // Public Methods
    // =========================
    public void RequestClient(int clientID) {
        _selectionService.RequestSelectedClient(clientID);
    }

    public void ToggleCompleted(int id) {
        var reminder = Filtered.FirstOrDefault(r => r.id == id);
        if (reminder != null) {
            var update = new UpdateReminderDTO();
            update.complete = !reminder.complete;
            _ = UpdateReminderAsync(id, update);
        }
    }

    public Reminder GetReminderByID(int id) {
        return Filtered.FirstOrDefault(r => r.id == id);
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

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadRemindersAsync() {
        var reminders = await _service.GetAllRemindersAsync();

        All.Clear();
        foreach (var reminder in reminders.OrderBy(o => o.date)) {
            All.Add(reminder);
        }
    }

    public async void DeleteReminderAsync(int id) {
        if (All.FirstOrDefault(r => r.id == id) is Reminder reminder) {
            await _service.DeleteReminderAsync(reminder.id);
            Remove(All, reminder);
            Remove(Filtered, reminder);
            Remove(ClientSpecific, reminder);
        }
    }

    public async Task ReloadReminderAsync(int id) {
        var reminder = await _service.GetReminderAsync(id);
        if (reminder == null) return;

        Replace(All, reminder);
        Replace(Filtered, reminder);
        Replace(ClientSpecific, reminder);
    }

    public async Task UpdateReminderAsync(int id, UpdateReminderDTO update) {
        await _service.UpdateReminderAsync(id, update);
        await ReloadReminderAsync(id);
    }

    private void Replace(ObservableCollection<Reminder> list, Reminder updated) {
        var index = list.IndexOf(list.FirstOrDefault(r => r.id == updated.id));
        if (index >= 0)
            list[index] = updated;
    }

    // =========================
    // Utility/Filtering Methods
    // =========================
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

    private void Remove(ObservableCollection<Reminder> list, Reminder reminder) {
        list.Remove(reminder);
    }

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
        EndText = Filtered.Count == 0
            ? "There are no reminders 静か"
            : "We have reached the end of the list 和";
    }
}
