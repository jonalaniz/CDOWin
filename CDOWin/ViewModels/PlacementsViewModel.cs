using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
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
    private readonly DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<PlacementSummaryDTO> _cache = [];

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<PlacementSummaryDTO> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Placement? Selected { get; set; }

    [ObservableProperty]
    public partial PlacementSummaryDTO? SelectedSummary { get; set; } 

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================

    public PlacementsViewModel(DataCoordinator dataCoordinator, IPlacementService service) {
        _service = service;
        _dataCoordinator = dataCoordinator;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) => ApplyFilter();

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadPlacementSummariesAsync(bool force = false) {
        var placements = await _dataCoordinator.GetPlacementSummariesAsync(force);
        if(placements == null) return;

        var snapshot = placements.OrderBy(o => o.Id).ToList().AsReadOnly();
        _cache = snapshot;
        ApplyFilter();
    }

    public async Task LoadSelectedPlacementAsync(string id) {
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
        string? previousSelection = Selected?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<PlacementSummaryDTO>(_cache);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _cache.Where(r =>
        (r.ClientName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (r.EmployerName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (r.SupervisorName ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (r.Position ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        OnUI(() => {
            Filtered = new ObservableCollection<PlacementSummaryDTO>(result);
            ReSelect(previousSelection);
        });
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(string? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(p => p.Id == id) is PlacementSummaryDTO selected)
            SelectedSummary = selected;
    }
}
