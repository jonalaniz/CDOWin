using Backstage.Composers;
using Backstage.Data;
using Backstage.Services;
using CDO.Core.DTOs.Admin;
using CDO.Core.ErrorHandling;
using CDO.Core.Services;
using CDO.Core.Services.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class ClientViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly AdminClientService _adminClientService;
    private readonly ClientService _clientService;
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
    public partial ClientHistory? SelectedClientHistory { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public ClientViewModel(DataCoordinator dataCoordinator, ClientSelectionService selectionService, AdminClientService adminClientService, ClientService clientService) {
        _dataCoordinator = dataCoordinator;
        _selectionService = selectionService;
        _adminClientService = adminClientService;
        _clientService = clientService;
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

    partial void OnSelectedChanged(AdminClientSummary? value) {
        if (value == null) return;
        _ = LoadSelectedClientHistory(value.Id);
    }

    // =========================
    // Client Export
    // =========================
    public async Task<Result> ExportClients() {
        var list = await _adminClientService.GetAllClientRecordsAsync();
        if (list == null) return Result.Fail(new AppError(ErrorKind.Unknown, "Client Export empty", null));
        var composer = new ClientComposer();
        composer.BuildCSV(list);
        return Result.Success();

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

    public async Task LoadStaleClientsAsync(bool force = false) {
        var clients = await _dataCoordinator.GetStaleClientsAsync(force);
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.UpdatedAt).ToList().AsReadOnly();
        OnUI(() => {
            StaleClients = new ObservableCollection<AdminClientSummary>(snapshot);
        });
    }

    public async Task LoadSelectedClientHistory(int id) {
        // TODO: Implement cancellation tokens

        var clientHistory = await _adminClientService.GetClientHistory(id);
        OnUI(() => SelectedClientHistory = clientHistory);
        
    }

    // =========================
    // Post Methods
    // =========================
    public async Task<Result> MarkClientActive(int id) {
        return await _clientService.MarkClientActiveAsync(id);
    }

    public async Task<Result> MarkClientInactive(int id) {
        return await _clientService.MarkClientInactiveAsync(id);
    }

    public async Task<Result> MarkClientTTW(int id) {
        return await _clientService.MarkClientTTWAsync(id);
    }

    public async Task<Result> UnmarkClientTTW(int id) {
        return await _clientService.MarkClientTTWAsync(id);
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

    public void ToggleActive(int clientId) {
        if (ClientSummaries.FirstOrDefault(c => c.Id == clientId) is not AdminClientSummary summary) return;
        var index = ClientSummaries.IndexOf(summary);

        summary = summary.ToggleActive();

        UpdateClient(summary, index);
    }

    public void ToggleTTW(int clientId) {
        if (ClientSummaries.FirstOrDefault(c => c.Id == clientId) is not AdminClientSummary summary) return;
        var index = ClientSummaries.IndexOf(summary);

        summary = summary.ToggleTTW();

        UpdateClient(summary, index);
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

    private void UpdateClient(AdminClientSummary summary, int index) {
        OnUI(() => {
            ClientSummaries[index] = summary;
            Selected = summary;
        });

        // Update in the background WITHOUT updating our cache
        _ = _dataCoordinator.GetClientSummariesAsync(force: true);
    }
}
