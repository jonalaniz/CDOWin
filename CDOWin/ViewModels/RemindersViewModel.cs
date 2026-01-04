using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.Views.Reminders;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class RemindersViewModel : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly IReminderService _service;
    private readonly ClientSelectionService _selectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<Reminder> _allReminders = [];
    private RemindersFilter _filter = RemindersFilter.All;

    // =========================
    // Public Properties / State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<Reminder> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> ClientSpecific { get; private set; } = [];

    [ObservableProperty]
    public partial string EndText { get; set; } = string.Empty;

    public RemindersFilter Filter {
        get => _filter;
        set {
            if (SetProperty(ref _filter, value)) {
                _dispatcher.TryEnqueue(ApplyFilterInternal);
            }
        }
    }

    // =========================
    // Constructor
    // =========================
    public RemindersViewModel(IReminderService service, ClientSelectionService clientSelectionService) {
        _service = service;
        _selectionService = clientSelectionService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _selectionService.SelectedClientChanged += OnClientChanged;
        _selectionService.NewReminderCreated += OnReminderCreated;
    }

    // =========================
    // Public Methods
    // =========================

    public IReadOnlyDictionary<DateTime, IReadOnlyList<Reminder>> GetRemindersByMonth(DateTime month) {
        return _allReminders
            .Where(r => r.Date.Month == month.Month)
            .GroupBy(r => r.Date.Date)
            .OrderBy(g => g.Key)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<Reminder>)g.ToList()
            );
    }

    public void RequestClient(int clientID) => _selectionService.RequestSelectedClient(clientID);

    public void DeferDate(int id, int days) {
        var reminder = Filtered.FirstOrDefault(r => r.Id == id);
        if (reminder != null) {
            var update = new UpdateReminderDTO { Date = reminder.Date.AddDays(days) };
            _ = UpdateReminderAsync(id, update);
        }
    }

    public void ToggleCompleted(int id) {
        var reminder = Filtered.FirstOrDefault(r => r.Id == id);
        if (reminder != null) {
            var update = new UpdateReminderDTO { Complete = !reminder.Complete };
            _ = UpdateReminderAsync(id, update);
        }
    }

    public Reminder? GetReminderByID(int id) => Filtered.FirstOrDefault(r => r.Id == id);

    public bool DateHasReminders(DateTime date) => _allReminders.Any(r => r.Date.Date == date.Date);

    public void ApplyDateFilter(DateTime date) {
        Filter = RemindersFilter.Date;
        var source = _allReminders
            .Where(r => r.Date.Date == date)
            .ToList()
            .AsReadOnly();

        _dispatcher.TryEnqueue(() => {
            Filtered.Clear();
            foreach (var reminder in source)
                Filtered.Add(reminder);

            UpdateEndText();
        });
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadRemindersAsync() {
        var reminders = await _service.GetAllRemindersAsync();
        if (reminders == null) return;

        var snapshot = reminders.OrderBy(r => r.Date).ToList().AsReadOnly();
        _allReminders = snapshot;

        _dispatcher.TryEnqueue(ApplyFilterInternal);
    }

    public async Task ReloadReminderAsync(int id) {
        var reminder = await _service.GetReminderAsync(id);
        if (reminder == null) return;

        var updated = _allReminders
            .Select(r => r.Id == id ? reminder : r)
            .ToList()
            .AsReadOnly();

        _allReminders = updated;
        _dispatcher.TryEnqueue(ApplyFilterInternal);
    }

    public async Task UpdateReminderAsync(int id, UpdateReminderDTO update) {
        await _service.UpdateReminderAsync(id, update);
        await ReloadReminderAsync(id);
    }

    public async Task DeleteReminderAsync(int id) {
        await _service.DeleteReminderAsync(id);

        _allReminders = _allReminders
            .Where(r => r.Id != id)
            .ToList()
            .AsReadOnly();

        _dispatcher.TryEnqueue(ApplyFilterInternal);
    }

    // =========================
    // Event Handlers
    // =========================
    private void OnClientChanged(Client? client) {
        Debug.WriteLine("On client changed");
        var source = client?.Reminders?
            .OrderBy(r => r.Date)
            .ToList()
            .AsReadOnly();

        if (source == null)  return;

        _dispatcher.TryEnqueue(() => {
            ClientSpecific.Clear();
            foreach (var reminder in source)
                ClientSpecific.Add(reminder);

            if (Filter == RemindersFilter.Client)
                ApplyFilterInternal();
        });
    }

    private void OnReminderCreated() {
        _ = LoadRemindersAsync();
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilterInternal() {
        IEnumerable<Reminder> source = _filter switch {
            RemindersFilter.All => _allReminders,
            RemindersFilter.Upcoming => _allReminders.Where(r => r.Date > DateTime.Now),
            RemindersFilter.Client => ClientSpecific,
            _ => Array.Empty<Reminder>()
        };

        Filtered.Clear();
        foreach (var reminder in source)
            Filtered.Add(reminder);

        UpdateEndText();
    }

    private void UpdateEndText() {
        EndText = Filtered.Count == 0
            ? "There are no Reminders 静か"
            : "We have reached the end of the list 和";
    }
}
