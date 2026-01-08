using CDO.Core.Interfaces;
using CDO.Core.Models;
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
    // Services / Dependencies
    // =========================
    private readonly IPlacementService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly PlacementSelectionService _selectionService;
    private readonly DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();

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
    public partial Placement? Selected { get; set; } = null;

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================

    public PlacementsViewModel(DataCoordinator dataCoordinator, IPlacementService service, PlacementSelectionService selectionService) {
        _service = service;
        _dataCoordinator = dataCoordinator;

        _selectionService = selectionService;
        _selectionService.NewPlacementCreated += OnNewPlacementCreated;
        _selectionService.PlacementSelected += OnPlacementSelected;
    }

    // =========================
    // Selection Handlers
    // =========================

    private void OnNewPlacementCreated() {
        _ = LoadPlacementsAsync();
    }

    private void OnPlacementSelected(string id) {
        var selected = _allPlacements.FirstOrDefault(p => p.Id == id);
        if (selected == null) return;

        _dispatcher.TryEnqueue(() => {
            Selected = selected;
            SearchQuery = "";
            //ApplyFilter();
        });
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
        var placements = await _dataCoordinator.GetPlacementsAsync();
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
            var index = Filtered
            .Select((p, i) => new { p, i })
            .FirstOrDefault(x => x.p.Id == id)?.i;

            if (index != null)
                Filtered[index.Value] = placement;

            Selected = placement;
        });
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
        string? previousSelection = Selected?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Placement>(_allPlacements);
            ReSelect(previousSelection);
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
        ReSelect(previousSelection);
    }

    private void ReSelect(string? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(p => p.Id == id) is Placement selected)
            Selected = selected;
    }
}
