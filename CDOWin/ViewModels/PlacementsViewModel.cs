using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
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

        var snapshot = placements.OrderBy(o => o.id).ToList().AsReadOnly();
        _allPlacements = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task ReloadPlacementAsync(string id) {
        var placement = await _service.GetPlacementAsync(id);
        if (placement == null) return;

        var updated = _allPlacements
            .Select(p => p.id == id ? placement : p)
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
        (r.clientName?.ToLower().Contains(query) ?? false)
        || (r.employer.name?.ToLower().Contains(query) ?? false)
        || (r.supervisor?.ToLower().Contains(query) ?? false)
        || (r.position?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<Placement>(result);
    }
}
