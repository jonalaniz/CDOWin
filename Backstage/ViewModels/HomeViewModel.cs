using Backstage.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.ErrorHandling;
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
    private readonly ClientService _service;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> RecentClients { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminClientNote> RecentNotes { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> StaleClients { get; private set; } = [];

    // =========================
    // Constructor
    // =========================
    public HomeViewModel(DataCoordinator dataCoordinator, ClientService clientService) {
        _dataCoordinator = dataCoordinator;
        _service = clientService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // Get Methods
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

    public async Task LoadStaleClientsAsync(bool force = false) {
        var clients = await _dataCoordinator.GetStaleClientsAsync(force);
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.UpdatedAt).ToList().AsReadOnly();
        OnUI(() => {
            StaleClients = new ObservableCollection<AdminClientSummary>(snapshot);
        });
    }

    // =========================
    // Post Methods
    // =========================
    public async Task<Result> MarkClientActive(int id) {
        return await _service.MarkClientActiveAsync(id);
    }

    public async Task<Result> MarkClientInactive(int id) {
        return await _service.MarkClientInactiveAsync(id);
    }

    // =========================
    // Utility Methods
    // =========================
    public void RemoveClient(int id) {
        if (StaleClients.FirstOrDefault(c => c.Id == id) is not AdminClientSummary client) return;
        OnUI(() => StaleClients.Remove(client));
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }
}
