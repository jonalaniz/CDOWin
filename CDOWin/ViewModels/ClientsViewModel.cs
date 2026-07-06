using CDO.Core.DTOs.Clients;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.DTOs.Placements;
using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDOWin.Composers;
using CDOWin.Data;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ClientsViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IClientService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly DataInvalidationService _invalidationService;
    private readonly ClientSelectionService _selectionService;
    private readonly PlacementSelectionService _placementSelectionService;
    private readonly ClientComposer _clientComposer = new();
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private CancellationTokenSource _ctSource = new();
    private CancellationTokenSource? _filterCts;

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<ClientSummary> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial ClientDetail? Selected { get; set; }

    [ObservableProperty]
    public partial ClientSummary? SelectedSummary { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<SADetail> Invoices { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<PlacementDetail> Placements { get; private set; } = [];

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsFiltered { get; set; } = true;

    // Notes Specific
    [ObservableProperty]
    public partial ObservableCollection<ClientNote> FilteredNotes { get; private set; } = [];

    [ObservableProperty]
    public partial string NotesSearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public ClientsViewModel(IClientService service,
        DataCoordinator dataCoordinator,
        ClientSelectionService clientSelectionService,
        PlacementSelectionService placementSelectionService,
        DataInvalidationService invalidationService) {
        _service = service;
        _dataCoordinator = dataCoordinator;
        _invalidationService = invalidationService;

        _selectionService = clientSelectionService;
        _placementSelectionService = placementSelectionService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _selectionService.ClientSelectionRequested += OnRequestSelectedClientChange;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) => _ = RefreshAsync();
    partial void OnNotesSearchQueryChanged(string value) => ApplyNotesFilter();
    partial void OnIsFilteredChanged(bool value) => _ = RefreshAsync();

    private void OnRequestSelectedClientChange(int clientId) {
        if (Selected != null && Selected.Id == clientId) return;
        SearchQuery = string.Empty;
        _ = LoadSelectedClientAsync(clientId);
    }

    partial void OnSelectedChanged(ClientDetail? value) {
        if (value == null) return;

        // Notify the selection service
        _selectionService.SelectedClient = value;

        // Setup Placements/SAs/Notes
        if (value.Sas is not null)
            SetupSAs(value.Sas);

        if (value.Placements is not null)
            SetupPlacements(value.Placements);

        ApplyNotesFilter();
    }

    // =========================
    // Public Methods
    // =========================
    public void NotifyNewReminderCreated() => _selectionService.NotifyNewReminderCreated();

    public void RequestPlacement(int placementID) {
        AppServices.Navigation.Navigate(Views.CDOFrame.Placements);
        _placementSelectionService.RequestSelectedPlacement(placementID);
    }

    // =========================
    // Get Methods
    // =========================
    public async Task LoadSelectedClientAsync(int id) {
        if (Selected != null && Selected.Id == id) return;
        ResetCancellationToken();

        var selectedClient = await _service.GetClientAsync(id, _ctSource.Token);
        Selected = selectedClient;
    }

    public async Task ReloadClientAsync() {
        if (Selected == null) return;
        Selected = await _service.GetClientAsync(Selected.Id);
        OnUI(() => UpdateSummaries());
    }

    // =========================
    // Post Methods
    // =========================
    public async Task<Result> MarkClientActive(int id) {
        var result = await _service.MarkClientActiveAsync(id);

        // Update our cache silently so data is in sync with active status
        _ = _dataCoordinator.GetClientsAsync(force: true);

        return result;
    }

    public async Task<Result> MarkClientInactive(int id) {
        InvalidateCache();
        var result = await _service.MarkClientInactiveAsync(id);

        // Update our cache silently so data is in sync with active status
        _ = _dataCoordinator.GetClientsAsync(force: true);

        return result;
    }

    public async Task<Result> MarkClientTTW(int id) {
        return await _service.MarkClientTTWAsync(id);
    }

    public async Task<Result> UnmarkClientTTW(int id) {
        return await _service.UnMarkClientTTWAsync(id);
    }


    // Update Methods
    public async Task<Result> UpdateClientAsync(ClientUpdate update) {
        if (Selected == null) return Result<ClientDetail>.Fail(new AppError(ErrorKind.Validation, "ClientDetail not selected.", null));

        var result = await _service.UpdateClientAsync(Selected.Id, update);
        if (result.IsSuccess) {
            await ReloadClientAsync();

            _ = Task.Run(() => {
                _clientComposer.ComposeClientToFile(Selected);
            });
        }
        return result;
    }

    public async Task<Result> UpdateNoteAsync(NoteUpdate update, int clientId, int noteId) {
        var result = await _service.UpdateClientNote(update, clientId, noteId);
        return result;
    }

    // Delete Methods
    public async Task<Result> DeleteClientAsync(int id) {
        var result = await _service.DeleteClientAsync(id);
        if (result.IsSuccess) OnUI(() => RemoveDeletedClient(id));
        InvalidateCache();
        return result;
    }

    public async Task<Result> DeleteNoteAsync(int clientId, int noteId) {
        var result = await _service.DeleteClientNoteAsync(clientId, noteId);
        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void SetupSAs(SADetail[] invoices) {
        var sortedInvoices = invoices
            .OrderBy(i => i.EndDate)
            .Reverse()
            .ToList();
        OnUI(() => {
            Invoices = new ObservableCollection<SADetail>(sortedInvoices);
        });
    }

    private void SetupPlacements(PlacementDetail[] placements) {
        var sortedPlacements = placements
            .OrderBy(p => p.HireDate)
            .Reverse()
            .ToList();
        OnUI(() => {
            Placements = new ObservableCollection<PlacementDetail>(sortedPlacements);
        });
    }

    public async Task RefreshAsync(bool force = false) {
        _filterCts?.Cancel();
        _filterCts = new CancellationTokenSource();
        var token = _filterCts.Token;

        try {
            await Task.Delay(150, token);
            if (token.IsCancellationRequested) return;

            var snapshot = await _dataCoordinator.GetClientsAsync(force);
            if (token.IsCancellationRequested) return;

            int? previousSelection = Selected?.Id;

            snapshot = IsFiltered ? snapshot.Where(i => i.Active == true).ToList() : snapshot;

            if (!string.IsNullOrWhiteSpace(SearchQuery)) {
                var query = SearchQuery.Trim().ToLower();
                snapshot = snapshot.Where(c =>
                (c.FirstName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.LastName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.Id.ToString() ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.FormattedAddress ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.Phone ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.Phone2 ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.Phone3 ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.EmploymentGoal ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (c.CaseID ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
                ).ToList();
            }

            OnUI(() => {
                Filtered = new ObservableCollection<ClientSummary>(
                    snapshot.OrderBy(c => c.Name)
                    );
                ReSelect(previousSelection);
            });
        } catch (OperationCanceledException) { }
    }

    private void InvalidateCache() {
        _invalidationService.InvalidateSAs();
        _invalidationService.InvalidatePlacements();
    }

    private void ApplyNotesFilter() {
        if (Selected == null || Selected.ClientNotes == null) return;

        IEnumerable<ClientNote> result = Selected.ClientNotes;
        if (!string.IsNullOrWhiteSpace(NotesSearchQuery)) {
            var query = NotesSearchQuery.Trim().ToLower();
            result = result.Where(n => n.Note.Contains(query, StringComparison.CurrentCultureIgnoreCase));
        }

        OnUI(() => {
            FilteredNotes = new ObservableCollection<ClientNote>(result);
        });
    }

    private void ResetCancellationToken() {
        _ctSource.Cancel();
        _ctSource.Dispose();
        _ctSource = new CancellationTokenSource();
    }

    private void RemoveDeletedClient(int id) {
        // Update our cache
        _ = _dataCoordinator.GetClientsAsync(force: true);

        if (Filtered.FirstOrDefault(c => c.Id == id) is ClientSummary client) {
            Filtered.Remove(client);
            Selected = null;
            SelectedSummary = null;
        }

        _invalidationService.InvalidateClients();
        _selectionService.SelectedClient = null;
    }

    private void UpdateSummaries() {
        if (Selected == null) return;

        // Update the cache
        _ = _dataCoordinator.GetClientsAsync();

        // Update our filter
        if (Filtered.FirstOrDefault(c => c.Id == Selected.Id) is ClientSummary client) {
            var i = Filtered.IndexOf(client);
            Filtered[i] = Selected.AsSummary();
            SelectedSummary = Filtered[i];
        }
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(c => c.Id == id) is ClientSummary selected)
            SelectedSummary = selected;
    }
}
