using CDO.Core.Interfaces;
using CDO.Core.Models;
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
    // Services / Dependencies
    // =========================
    private readonly IPlacementService _service;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<Placement> _allPlacements = [];

    // =========================
    // Public Property / State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<Placement> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Placement? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;


    // =========================
    // Constructor
    // =========================
    public PlacementsViewModel(IPlacementService service) {
        _service = service;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) {
        if (_dispatcher.HasThreadAccess)
            ApplyFilter();
        else
            _dispatcher.TryEnqueue(ApplyFilter);
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadPlacementsAsync() {
        var placements = await _service.GetAllPlacementsAsync();
        if (placements == null) return;

        var snapshot = placements.OrderBy(o => o.Id).ToList().AsReadOnly();
        _allPlacements = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task ReloadPlacementAsync(string id) {
        var placement = await _service.GetPlacementAsync(id);
        if (placement == null) return;

        var updated = _allPlacements
            .Select(p => p.Id == id ? placement : p)
            .ToList()
            .AsReadOnly();

        _allPlacements = updated;
        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
            Selected = placement;
        });

        Selected = placement;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Placement>(_allPlacements);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allPlacements.Where(r =>
        (r.ClientName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) || 
        (r.Employer?.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) || 
        (r.Supervisor ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase) || 
        (r.Position ?? "").ToLower().Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        Filtered = new ObservableCollection<Placement>(result);
    }
}
