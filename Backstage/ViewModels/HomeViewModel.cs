using Backstage.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Clients.Notes;
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

public partial class HomeViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientService _clientService;
    private readonly ServiceAuthorizationService _saService;
    private readonly ReminderService _reminderService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> RecentClients { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminClientNote> RecentNotes { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> Reminders { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminSASummary> ExpiringSAs { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> StaleClients { get; private set; } = [];

    // =========================
    // Constructor
    // =========================
    public HomeViewModel(DataCoordinator dataCoordinator, ClientService clientService, ServiceAuthorizationService saService, ReminderService reminderService) {
        _dataCoordinator = dataCoordinator;
        _clientService = clientService;
        _saService = saService;
        _reminderService = reminderService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadRecentClientsAsync(bool force = false) {
        var clients = await _dataCoordinator.GetRecentClientsAsync(force);
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.UpdatedAt).ToList().AsReadOnly();
        OnUI(() => {
            RecentClients = new ObservableCollection<AdminClientSummary>(snapshot);
        });
    }

    public async Task LoadRecentNotesAsync(bool force = false) {
        var notes = await _dataCoordinator.GetRecentNotesAsync(force);
        if (notes == null) return;

        var snapshot = notes.OrderBy(n => n.Date).ToList().AsReadOnly();
        OnUI(() => {
            RecentNotes = new ObservableCollection<AdminClientNote>(snapshot);
        });
    }

    public async Task LoadRecentRemindersAsync(bool force = false) {
        var reminders = await _dataCoordinator.GetRemindersAsync(force);
        if (reminders == null) return;

        var snapshot = reminders.OrderBy(r => r.Date).ToList().AsReadOnly();
        OnUI(() => {
            Reminders = new ObservableCollection<Reminder>(snapshot);
        });
    }

    public async Task LoadExpiringSAsAsync(bool force = false) {
        var sas = await _dataCoordinator.GetExpiringSAsAsync(force);
        if (sas == null) return;

        var snapshot = sas.OrderBy(s => s.EndDate).ToList().AsReadOnly();
        OnUI(() => {
            ExpiringSAs = new ObservableCollection<AdminSASummary>(snapshot);
        });
    }

    public async Task LoadStaleClientsAsync(bool force = false) {
        var clients = await _dataCoordinator.GetStaleClientsAsync(force);
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.UpdatedAt).ToList().AsReadOnly();
        OnUI(() => {
            StaleClients = new ObservableCollection<AdminClientSummary>(snapshot);
        });
    }

    // Post Methods

    public async Task<Result> MarkClientActive(int id) {
        return await _clientService.MarkClientActiveAsync(id);
    }

    public async Task<Result> MarkClientInactive(int id) {
        return await _clientService.MarkClientInactiveAsync(id);
    }

    public async Task<Result> MarkSABilled(int id) {
        return await _saService.MarkSABilled(id);
    }

    public async Task<Result> MarkSAUnbilled(int id) {
        return await _saService.MarkSAUnbilled(id);
    }

    public async Task<Result<Reminder>> CreateReminderAsync(NewReminder reminder) {
        return await _reminderService.CreateRemindersAsync(reminder);
    }

    // =========================
    // Utility Methods
    // =========================

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    public AdminSASummary? SAForId(int id) {
        return ExpiringSAs.FirstOrDefault(sa => sa.Id == id);
    }

    public void RemoveClient(int id) {
        if (StaleClients.FirstOrDefault(c => c.Id == id) is not AdminClientSummary client) return;
        StaleClients.Remove(client);
    }

    public void RemoveSA(int id) {
        if (ExpiringSAs.FirstOrDefault(sa => sa.Id == id) is not AdminSASummary sa) return;
        ExpiringSAs.Remove(sa);
    }
}
