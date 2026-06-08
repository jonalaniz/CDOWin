using Backstage.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.Services.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class ClientViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly AdminClientService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================

    private IReadOnlyList<AdminClientSummary> _recentClients = [];
    private IReadOnlyList<AdminClientSummary> _staleClients = [];
    private IReadOnlyList<ClientNote> _recentNotes = [];

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> RecentClients { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> StaleClients { get; private set; } = [];

    // =========================
    // Constructor
    // =========================
    public ClientViewModel(DataCoordinator dataCoordinator, AdminClientService clientService) {
        _dataCoordinator = dataCoordinator;
        _service = clientService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // Public Methods
    // =========================
    public List<AdminClientSummary> CachedRecentClients() => _recentClients.ToList();
    public List<AdminClientSummary> CachedStaleClients() => _staleClients.ToList();

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

    public async Task LoadStaleClientsAsync(bool force = false) {
        var clients = await _dataCoordinator.GetStaleClientsAsync(force);
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.UpdatedAt).ToList().AsReadOnly();
        _staleClients = snapshot;
        OnUI(() => {
            StaleClients = new ObservableCollection<AdminClientSummary>(snapshot);
        });
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }
}
