using Backstage.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Reminders;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;
using CDO.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class ReminderViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly ReminderService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<AdminReminderDetail> Reminders { get; private set; } = [];

    // =========================
    // Constructor
    // =========================
    public ReminderViewModel(DataCoordinator dataCoordinator, ReminderService reminderService) {
        _dataCoordinator = dataCoordinator;
        _service = reminderService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // Get Methods
    // =========================
    public async Task LoadRecentRemindersAsync(bool force = false) {
        var reminders = await _dataCoordinator.GetRemindersAsync(force);
        if (reminders == null) return;

        var snapshot = reminders.OrderBy(r => r.Date).ToList().AsReadOnly();
        OnUI(() => {
            Reminders = new ObservableCollection<AdminReminderDetail>(snapshot);
        });
    }

    // =========================
    // Post Methods
    // =========================
    public async Task<Result<Reminder>> CreateReminderAsync(NewReminder reminder) {
        return await _service.CreateRemindersAsync(reminder);
    }

    // =========================
    // Utility Methods
    // =========================
    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }
}
