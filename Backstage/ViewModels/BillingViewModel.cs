using Backstage.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Placements;
using CDO.Core.Services.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
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
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<AdminSASummary> SAs { get; private set; } = [];

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
    // CRUD Methods
    // =========================
    public async Task LoadSASummariesAsync(bool force = false) {
        var sas = await _dataCoordinator.GetUnbilledSAsAsync(force);
        if (sas == null) return;

        var snapshot = sas.OrderBy(s => s.StartDate).ToList().AsReadOnly();
        OnUI(() => {
            SAs = new ObservableCollection<AdminSASummary>(snapshot);
        });
    }

    public async Task LoadPlacementSummariesAsync(bool force = false) {
        var placements = await _dataCoordinator.GetUnbilledPlacementsAsync(force);
        if (placements == null) return;

        var snapshot = placements.OrderBy(p => p.Id).ToList().AsReadOnly();
        OnUI(() => {
            Placements = new ObservableCollection<PlacementSummary>(snapshot);
        });
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }
}
