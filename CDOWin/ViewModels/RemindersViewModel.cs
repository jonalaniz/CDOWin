using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
using CDOWin.Services;
using CDOWin.Views.Reminders;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
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
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientSelectionService _selectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<Reminder> _allReminders = [];
    private RemindersFilter _filter = RemindersFilter.All;
    private readonly DispatcherTimer _refreshTimer;
    private DateTime _selectedDate = DateTime.Now;

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
                _dispatcher.TryEnqueue(ApplyFilter);
            }
        }
    }

    // =========================
    // Constructor
    // =========================
    public RemindersViewModel(DataCoordinator dataCoordinator, IReminderService service, ClientSelectionService clientSelectionService) {
        _service = service;
        _dataCoordinator = dataCoordinator;

        _selectionService = clientSelectionService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _selectionService.SelectedClientChanged += OnClientChanged;
        _selectionService.NewReminderCreated += OnReminderCreated;

        _refreshTimer = new DispatcherTimer {
            Interval = TimeSpan.FromSeconds(30)
        };

        _refreshTimer.Tick += async (_, _) => await LoadRemindersAsync();
        _refreshTimer.Start();
    }

    // =========================
    // Public Methods
    // =========================

    public IReadOnlyDictionary<DateTime, IReadOnlyList<Reminder>> GetRemindersByMonth(DateTime month) {
        var dict = new Dictionary<DateTime, IReadOnlyList<Reminder>>();
        foreach (var group in _allReminders
            .Where(r => r.Date.Month == month.Month)
            .GroupBy(r => r.Date.Date)
            .OrderBy(g => g.Key)) {
            dict[group.Key] = group.ToList();
        }
        return dict;
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

    public Reminder? GetReminderByID(int id) => _allReminders.FirstOrDefault(r => r.Id == id);

    public bool DateHasReminders(DateTime date) => _allReminders.Any(r => r.Date.Date == date.Date);

    public void ApplyDateFilter(DateTime date) {
        Filter = RemindersFilter.Date;
        _selectedDate = date;
        ApplyFilter();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadRemindersAsync() {
        var reminders = await _dataCoordinator.GetRemindersAsync();
        if (reminders == null) return;

        var snapshot = reminders.OrderBy(r => r.Date).ToList().AsReadOnly();
        _allReminders = snapshot;

        _dispatcher.TryEnqueue(ApplyFilter);
    }

    public async Task ReloadReminderAsync(int id) {
        var reminder = await _service.GetReminderAsync(id);
        if (reminder == null) return;

        var updated = _allReminders
            .Select(r => r.Id == id ? reminder : r)
            .ToList()
            .AsReadOnly();

        _allReminders = updated;

        if (Filter == RemindersFilter.Client) {
            _dispatcher.TryEnqueue(() => ClientSpecific = UpdateReminder(id, reminder, ClientSpecific));
        }

        _dispatcher.TryEnqueue(ApplyFilter);
    }

    public async Task<Result<Reminder>> UpdateReminderAsync(int id, UpdateReminderDTO update) {
        var result = await _service.UpdateReminderAsync(id, update);

        if (!result.IsSuccess) return result;

        await ReloadReminderAsync(id);
        return result;
    }

    public async Task DeleteReminderAsync(int id) {
        await _service.DeleteReminderAsync(id);

        _allReminders = _allReminders
            .Where(r => r.Id != id)
            .ToList()
            .AsReadOnly();

        if (Filter == RemindersFilter.Client) {
            _dispatcher.TryEnqueue(() => ClientSpecific = RemoveReminder(id, ClientSpecific));
        }

        _dispatcher.TryEnqueue(ApplyFilter);
    }

    private static ObservableCollection<Reminder> UpdateReminder(int id, Reminder reminder, ObservableCollection<Reminder> collection) {
        return new ObservableCollection<Reminder>(collection.Select(r => r.Id == id ? reminder : r));
    }

    private static ObservableCollection<Reminder> RemoveReminder(int id, ObservableCollection<Reminder> collection) {
        return new ObservableCollection<Reminder>(collection.Where(r => r.Id != id));
    }

    // =========================
    // Event Handlers
    // =========================
    private void OnClientChanged(Client? client) {
        var source = client?.Reminders?
            .OrderBy(r => r.Date)
            .ToList()
            .AsReadOnly();

        if (source == null) return;

        _dispatcher.TryEnqueue(() => {
            ClientSpecific.Clear();
            foreach (var reminder in source)
                ClientSpecific.Add(reminder);

            if (Filter == RemindersFilter.Client)
                ApplyFilter();
        });
    }

    private void OnReminderCreated() {
        _ = LoadRemindersAsync();
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
        Debug.WriteLine(Filter.ToString());
        IEnumerable<Reminder> source = _filter switch {
            RemindersFilter.All => _allReminders,
            RemindersFilter.Upcoming => _allReminders.Where(r => r.Date > DateTime.Now),
            RemindersFilter.Client => ClientSpecific,
            RemindersFilter.Date => DateSpecifc(),
            _ => []
        };

        Filtered.Clear();
        foreach (var reminder in source)
            Filtered.Add(reminder);

        UpdateEndText();
    }

    private IReadOnlyList<Reminder> DateSpecifc() {
        return _allReminders
            .Where(r => r.Date.Date == _selectedDate.Date)
            .ToList()
            .AsReadOnly();
    }

    private void UpdateEndText() {
        EndText = Filtered.Count == 0
            ? "There are no Reminders 静か"
            : "We have reached the end of the list 和";
    }
}
