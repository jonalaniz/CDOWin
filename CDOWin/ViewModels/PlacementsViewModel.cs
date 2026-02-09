using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDOWin.Data;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    private IReadOnlyList<PlacementSummary> _cache = [];

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
    public partial bool IsFiltered { get; set; } = false;

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
    partial void OnSearchQueryChanged(string value) => ApplyFilter();
    partial void OnIsFilteredChanged(bool value) => ApplyFilter();

    private void OnRequestSelectedPlacementChange(int placementID) {
        if (Selected != null && Selected.Id == placementID)  return;
        SearchQuery = string.Empty;
        ApplyFilter();
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

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadPlacementSummariesAsync(bool force = false) {
        var placements = await _dataCoordinator.GetPlacementSummariesAsync(force);
        if (placements == null) return;

        var snapshot = placements.OrderBy(o => o.HireDate).ToList().AsReadOnly();
        _cache = snapshot;
        ApplyFilter();
    }

    public async Task LoadSelectedPlacementAsync(int id) {
        if (Selected != null && Selected.Id == id) return;

        var placement = await _service.GetPlacementAsync(id);

        OnUI(() => { Selected = placement; });
    }

    public async Task<Result<bool>> DeleteSelectedPlacement() {
        if (Selected == null) return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No Placement selected.", null, null));
        var result = await _service.DeletePlacementAsync(Selected.Id);

        if (result.IsSuccess) {
            Selected = null;
            _ = LoadPlacementSummariesAsync(force: true);
        }

        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
        int? previousSelection = Selected?.Id;
        var filterDate = IsFiltered ? DateTime.Today : DateTime.MinValue;

        IEnumerable<PlacementSummary> result = _cache.Where(p => (p.HireDate ?? DateTime.MinValue.AddDays(1)) >= filterDate);

        if (!string.IsNullOrWhiteSpace(SearchQuery)) {
            var query = SearchQuery.Trim().ToLower();
            result = _cache.Where(r =>
            (r.ClientName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (r.EmployerName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (r.SupervisorName ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (r.Position ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase)
            );
        }

        OnUI(() => {
            Filtered = new ObservableCollection<PlacementSummary>(result);
            ReSelect(previousSelection);
        });
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
