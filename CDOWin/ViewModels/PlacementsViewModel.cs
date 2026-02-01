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
    private IReadOnlyList<Placement> _cache = [];

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<Placement> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Placement? Selected { get; set; } = null;

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
    public async Task LoadPlacementsAsync(bool force = false) {
        var placements = await _dataCoordinator.GetPlacementsAsync(force);
        if (placements == null) return;

        var snapshot = placements.OrderBy(o => o.Id).ToList().AsReadOnly();
        _cache = snapshot;
        ApplyFilter();
    }

    public async Task ReloadPlacementAsync(string id) {
        var placement = await _service.GetPlacementAsync(id);
        if (placement == null) return;

        var updated = _cache
            .Select(p => p.Id == id ? placement : p)
            .ToList()
            .AsReadOnly();
        _cache = updated;

        var index = Filtered
            .Select((p, i) => new { p, i })
            .FirstOrDefault(x => x.p.Id == id)?.i;

        OnUI(() => {
            if (index != null) Filtered[index.Value] = placement;
            Selected = placement;
        });
    }

    public async Task<Result<bool>> DeleteSelectedPlacement() {
        if (Selected == null) return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No Placement selected.", null, null));
        var result = await _service.DeletePlacementAsync(Selected.Id);

        if (result.IsSuccess) {
            Selected = null;
            _ = LoadPlacementsAsync(force: true);
        }

        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
        string? previousSelection = Selected?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Placement>(_cache);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _cache.Where(r =>
        (r.ClientName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (r.Employer?.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (r.Supervisor ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (r.Position ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        OnUI(() => {
            Filtered = new ObservableCollection<Placement>(result);
            ReSelect(previousSelection);
        });
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(string? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(p => p.Id == id) is Placement selected)
            Selected = selected;
    }
}
