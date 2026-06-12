using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDOWin.Data;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class PlacementsViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IPlacementService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientSelectionService _clientSelectionService;
    private readonly CounselorSelectionService _counselorSelectionService;
    private readonly EmployerSelectionService _employerSelectionService;
    private readonly PlacementSelectionService _placementSelectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private CancellationTokenSource? _filterCts;

    private bool Reversed { get; set; } = true;

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<PlacementSummary> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial PlacementDetail? Selected { get; set; }

    [ObservableProperty]
    public partial PlacementSummary? SelectedSummary { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsFiltered { get; set; } = true;

    // =========================
    // Constructor
    // =========================
    public PlacementsViewModel(
        DataCoordinator dataCoordinator,
        IPlacementService service,
        ClientSelectionService clientSelectionService,
        CounselorSelectionService counselorSelectionService,
        EmployerSelectionService employerSelectionService,
        PlacementSelectionService placementSelectionService
        ) {
        _service = service;
        _dataCoordinator = dataCoordinator;

        _clientSelectionService = clientSelectionService;
        _counselorSelectionService = counselorSelectionService;
        _employerSelectionService = employerSelectionService;
        _placementSelectionService = placementSelectionService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _placementSelectionService.PlacementSelectionRequested += OnRequestSelectedPlacementChange;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) => _ = RefreshAsync();
    partial void OnIsFilteredChanged(bool value) => _ = RefreshAsync();

    private void OnRequestSelectedPlacementChange(int placementID) {
        if (Selected != null && Selected.Id == placementID) return;
        SearchQuery = string.Empty;
        RefreshAsync();
        _ = LoadSelectedPlacementAsync(placementID);
    }

    // =========================
    // Public Methods
    // =========================
    public void RequestClient(int clientID) {
        AppServices.Navigation.Navigate(Views.CDOFrame.Clients);
        _clientSelectionService.RequestSelectedClient(clientID);
    }

    public void RequestCounselor(int counselorID) {
        AppServices.Navigation.Navigate(Views.CDOFrame.Counselors);
        _counselorSelectionService.RequestSelectedCounselor(counselorID);
    }

    public void RequestEmployer(int employerID) {
        AppServices.Navigation.Navigate(Views.CDOFrame.Employers);
        _employerSelectionService.RequestSelectedEmployer(employerID);
    }

    public async Task ToggleSortAsync() {
        Reversed = !Reversed;
        await RefreshAsync();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadSelectedPlacementAsync(int id) {
        if (Selected != null && Selected.Id == id) return;

        var placement = await _service.GetPlacementAsync(id);

        OnUI(() => { Selected = placement; });
    }

    public async Task ReloadPlacementAsync() {
        if (Selected == null) return;
        Selected = await _service.GetPlacementAsync(Selected.Id);
    }

    public async Task<Result> DeleteSelectedPlacement() {
        if (Selected == null) return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No Placement selected.", null, null));
        var id = SelectedSummary.Id;
        var result = await _service.DeletePlacementAsync(id);

        if (result.IsSuccess) OnUI(() => RemoveDeletedPlacement(id));

        return result;
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

            var snapshot = await _dataCoordinator.GetPlacementSummariesAsync(force);
            if (token.IsCancellationRequested) return;

            int? previousSelection = Selected?.Id;

            snapshot = IsFiltered ? snapshot.Where(i => i.Active == true).ToList() : snapshot;

            snapshot = snapshot.OrderBy(p => p.HireDate).ToList();
            if (Reversed) snapshot = snapshot.Reverse().ToList();

            if (!string.IsNullOrWhiteSpace(SearchQuery)) {
                var query = SearchQuery.Trim().ToLower();
                snapshot = snapshot.Where(r =>
                (r.ClientName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (r.EmployerName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (r.SupervisorName ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (r.Position ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase)
                ).ToList();
            }

            OnUI(() => {
                Filtered = new ObservableCollection<PlacementSummary>(snapshot);
                ReSelect(previousSelection);
            });
        } catch (OperationCanceledException) { }
    }

    private void RemoveDeletedPlacement(int id) {
        // Update our cache
        _ = _dataCoordinator.GetClientsAsync(force: true);

        if (Filtered.FirstOrDefault(p => p.Id == id) is PlacementSummary placement) {
            Filtered.Remove(placement);
            Selected = null;
        }
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(p => p.Id == id) is PlacementSummary selected)
            SelectedSummary = selected;
    }
}
