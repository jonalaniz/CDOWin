using Backstage.Data;
using Backstage.Services;
using CDO.Core.DTOs.Admin;
using CDO.Core.Services.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class ClientViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly AdminClientService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientSelectionService _selectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private CancellationTokenSource? _filterCts;

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> RecentClients { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> StaleClients { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> ClientSummaries { get; private set; } = [];

    [ObservableProperty]
    public partial AdminClientSummary? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public ClientViewModel(DataCoordinator dataCoordinator, ClientSelectionService selectionService, AdminClientService clientService) {
        _dataCoordinator = dataCoordinator;
        _selectionService = selectionService;
        _service = clientService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _selectionService.ClientSelectionRequested += OnRequesteSelectedClientChange;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) => _ = RefreshAsync();

    private void OnRequesteSelectedClientChange(int clientId) {
        if (Selected != null && Selected.Id == clientId) return;
        SearchQuery = string.Empty;
        OnUI(() => {
            if (ClientSummaries.FirstOrDefault(c => c.Id == clientId) is AdminClientSummary summary)
                Selected = summary;
        }
        );
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

    public async Task LoadStaleClientsAsync(bool force = false) {
        var clients = await _dataCoordinator.GetStaleClientsAsync(force);
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.UpdatedAt).ToList().AsReadOnly();
        OnUI(() => {
            StaleClients = new ObservableCollection<AdminClientSummary>(snapshot);
        });
    }

    // =========================
    // Utility / Filtering
    // =========================

    public async Task RefreshAsync(bool force = false) {
        _filterCts?.Cancel();
        _filterCts = new CancellationTokenSource();
        var token = _filterCts.Token;

        try {
            await Task.Delay(150, token);
            if (token.IsCancellationRequested) return;

            var snapshot = await _dataCoordinator.GetClientSummariesAsync(force);
            if (token.IsCancellationRequested) return;

            int? previousSelection = Selected?.Id;

            if (!string.IsNullOrWhiteSpace(SearchQuery)) {
                var query = SearchQuery.Trim().ToLower();
                snapshot = snapshot.Where(c =>
                (c.FirstName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.LastName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.Id.ToString() ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.FormattedAddress ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.CaseID ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
                ).ToList();
            }

            OnUI(() => {
                ClientSummaries = new ObservableCollection<AdminClientSummary>(
                    snapshot.OrderBy(c => c.Name)
                    );
                ReSelect(previousSelection);
            });
        } catch (OperationCanceledException) { }
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (ClientSummaries.FirstOrDefault(c => c.Id == id) is AdminClientSummary selected)
            Selected = selected;
    }
}
