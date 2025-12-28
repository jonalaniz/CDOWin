using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
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

    // =========================
    // View State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<Placement> All { get; private set; } = [];

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
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadPlacementsAsync() {
        var placements = await _service.GetAllPlacementsAsync();
        List<Placement> SortedPlacements = placements.OrderBy(o => o.clientID).ToList();
        All.Clear();

        foreach (var placement in SortedPlacements) {
            All.Add(placement);
        }

        ApplyFilter();
    }

    public async Task ReloadPlacementAsync(string id) {
        var placement = await _service.GetPlacementAsync(id);
        if (placement == null) return;

        var index = All.IndexOf(All.First(r => r.id == placement.id));
        All[index] = placement;
        Selected = placement;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Placement>(All);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = All.Where(r =>
        (r.clientName?.ToLower().Contains(query) ?? false)
        || (r.employer.name?.ToLower().Contains(query) ?? false)
        || (r.supervisor?.ToLower().Contains(query) ?? false)
        || (r.position?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<Placement>(result);
    }
}
