using Backstage.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.Models;
using CDO.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;

namespace Backstage.ViewModels; 
public partial class HomeViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly DataCoordinator _dataCoordinator;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<AdminClientSummary> _recentClients = [];
    private IReadOnlyList<ClientNote> _recentNotes = [];
    private IReadOnlyList<Reminder> _reminders = [];

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> RecentClients { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<ClientNote> RecentNotes { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Reminder> Reminders { get; private set; } = [];

    // =========================
    // Constructor
    // =========================
    public HomeViewModel(DataCoordinator dataCoordinator) {
        _dataCoordinator = dataCoordinator;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadRecentClientsAsync(bool force = false) {
        var clients = await _dataCoordinator.GetRecentClientsAsync(force);
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.UpdatedAt).ToList().AsReadOnly();
        _recentClients = snapshot;
        OnUI(() => {
            RecentClients = new ObservableCollection<AdminClientSummary>(snapshot);
        });
    }

    public async Task LoadRecentNotesAsync(bool force = false) {
        var notes = await _dataCoordinator.GetRecentNotesAsync(force);
        if (notes == null) return;

        var snapshot = notes.OrderBy(n => n.Date).ToList().AsReadOnly();
        _recentNotes = snapshot;
        OnUI(() => {
            RecentNotes = new ObservableCollection<ClientNote>(snapshot);
        });
    }
    
    public async Task LoadRecentRemindersAsync(bool force = false) {
        var reminders = await _dataCoordinator.GetRemindersAsync(force);
        if (reminders == null) return;

        var snapshot = reminders.OrderBy(r => r.Date).ToList().AsReadOnly();
        _reminders = snapshot;
        OnUI(() => {
            Reminders = new ObservableCollection<Reminder>(snapshot);
        });
    }


    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }
}
