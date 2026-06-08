using Backstage.Data;
using CDO.Core.DTOs.Placements;
using CDO.Core.DTOs.SAs;
using CDO.Core.Services.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class BillingViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly BillingService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<SASummary> _saCache = [];
    private IReadOnlyList<PlacementSummary> _placementCache = [];

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<SASummary> SAs { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<PlacementSummary> Placements { get; private set; } = [];

    // =========================
    // Constructor
    // =========================
    public BillingViewModel(DataCoordinator dataCoordinator, BillingService billingService) {
        _dataCoordinator = dataCoordinator;
        _service = billingService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // Public Methods
    // =========================
    public List<SASummary> UnbilledSAs() => _saCache.ToList();
    public List<PlacementSummary> UnbilledPlacements() => _placementCache.ToList();

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadSASummariesAsync(bool force = false) {
        var sas = await _dataCoordinator.GetUnbilledSAsAsync(force);
        if (sas == null) return;

        var snapshot = sas.OrderBy(s => s.StartDate).ToList().AsReadOnly();
        _saCache = snapshot;
        OnUI(() => {
            SAs = new ObservableCollection<SASummary>(snapshot);
        });
    }

    public async Task LoadPlacementSummariesAsync(bool force = false) {
        var placements = await _dataCoordinator.GetUnbilledPlacementsAsync(force);
        if (placements == null) return;

        var snapshot = placements.OrderBy(p => p.Id).ToList().AsReadOnly();
        _placementCache = snapshot;
        OnUI(() => {
            Placements = new ObservableCollection<PlacementSummary>(snapshot);
        });
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }
}
