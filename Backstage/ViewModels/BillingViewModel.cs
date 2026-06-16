using Backstage.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.ErrorHandling;
using CDO.Core.Services;
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
    private readonly BillingService _billingService;
    private readonly ServiceAuthorizationService _saService;
    private readonly DataCoordinator _dataCoordinator;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<AdminSASummary> ExpiringSAs { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<AdminSASummary> UnbilledSAs { get; private set; } = [];


    // =========================
    // Constructor
    // =========================
    public BillingViewModel(DataCoordinator dataCoordinator, BillingService billingService, ServiceAuthorizationService saService) {
        _dataCoordinator = dataCoordinator;
        _billingService = billingService;
        _saService = saService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // Get Methods
    // =========================
    public async Task LoadUnbilledSAs(bool force = false) {
        var sas = await _dataCoordinator.GetUnbilledSAsAsync(force);
        if (sas == null) return;

        var snapshot = sas.OrderBy(s => s.StartDate).ToList().AsReadOnly();
        OnUI(() => {
            UnbilledSAs = new ObservableCollection<AdminSASummary>(snapshot);
        });
    }

    public async Task LoadExpiringSAsAsync(bool force = false) {
        var sas = await _dataCoordinator.GetExpiringSAsAsync(force);
        if (sas == null) return;

        var snapshot = sas.OrderBy(s => s.EndDate).ToList().AsReadOnly();
        OnUI(() => {
            ExpiringSAs = new ObservableCollection<AdminSASummary>(snapshot);
        });
    }

    // =========================
    // Post Methods
    // =========================
    public async Task<Result> MarkSABilled(int id) {
        return await _saService.MarkSABilled(id);
    }

    public async Task<Result> MarkSAUnbilled(int id) {
        return await _saService.MarkSAUnbilled(id);
    }

    // =========================
    // Utility Methods
    // =========================
    public AdminSASummary? ExpiredSA(int id) {
        return ExpiringSAs.FirstOrDefault(sa => sa.Id == id);
    }

    public AdminSASummary? UnbilledSA(int id) {
        return UnbilledSAs.FirstOrDefault(sa => sa.Id == id);
    }

    public void RemoveExpiredSA(int id) {
        if (ExpiringSAs.FirstOrDefault(sa => sa.Id == id) is not AdminSASummary sa) return;
        OnUI(() => ExpiringSAs.Remove(sa));
    }

    public void RemoveUnbilledSA(int id) {
        if (UnbilledSAs.FirstOrDefault(sa => sa.Id == id) is not AdminSASummary sa) return;
        OnUI(() => UnbilledSAs.Remove(sa));
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }
}
