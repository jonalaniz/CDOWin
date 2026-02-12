using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Composers;
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

public partial class ServiceAuthorizationsViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientSelectionService _clientSelectionService;
    private readonly CounselorSelectionService _counselorSelectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<Invoice> _cache = [];

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<Invoice> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Invoice? SelectedSummary { get; set; }

    [ObservableProperty]
    public partial Invoice? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsFiltered { get; set; } = false;

    // =========================
    // Constructor
    // =========================

    public ServiceAuthorizationsViewModel(
        DataCoordinator dataCoordinator,
        IServiceAuthorizationService service,
        ClientSelectionService clientSelectionService,
        CounselorSelectionService counselorSelectionService) {
        _service = service;
        _dataCoordinator = dataCoordinator;


        _clientSelectionService = clientSelectionService;
        _counselorSelectionService = counselorSelectionService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) => ApplyFilter();
    partial void OnIsFilteredChanged(bool value) => ApplyFilter();

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

    // =========================
    // Export Methods
    // =========================
    public Task<Result<string>> ExportSelectedAsync() {
        var tcs = new TaskCompletionSource<Result<string>>();

        if (Selected == null) {
            tcs.SetResult(Result<string>.Fail(new AppError(ErrorKind.Validation, "No SA Selected.")));
            return tcs.Task;
        }

        if (Selected.Client is null) {
            tcs.SetResult(Result<string>.Fail(new AppError(ErrorKind.Validation, "This shouldn't be possible")));
            return tcs.Task;
        }

        var composer = new ServiceAuthorizationComposer(Selected);
        return composer.Compose();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadServiceAuthorizationsAsync(bool force = false) {
        var serviceAuthorizations = await _dataCoordinator.GetSAsAsync();
        if (serviceAuthorizations == null) return;

        var snapshot = serviceAuthorizations.OrderBy(o => o.EndDate).ToList().AsReadOnly();
        _cache = snapshot;
        ApplyFilter();
    }

    public async Task LoadSelectedSAAsync(int id) {
        if (Selected != null && Selected.Id == id) return;

        var selectedSA = await _service.GetServiceAuthorizationAsync(id);
        Selected = selectedSA;
    }

    public async Task<Result> DeleteSelectedSA() {
        if (SelectedSummary == null) return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No SA selected.", null, null));
        var id = SelectedSummary.Id;
        var result = await _service.DeleteServiceAuthorizationAsync(id);

        if (result.IsSuccess) {
            OnUI(() => {
                Selected = null;
                SelectedSummary = null;
            });
            _ = LoadServiceAuthorizationsAsync(force: true);
        }

        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================
    void ApplyFilter() {
        int? previousSelection = SelectedSummary?.Id;
        var filterDate = IsFiltered ? DateTime.Today : DateTime.MinValue;

        IEnumerable<Invoice> result = _cache.Where(i => i.EndDate >= filterDate);

        if (!string.IsNullOrWhiteSpace(SearchQuery)) {
            var query = SearchQuery.Trim().ToLower();
            result = result.Where(i =>
            (i.ClientName).Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (i.CounselorName).Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (i.ServiceAuthorizationNumber).Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (i.Description).Contains(query, StringComparison.CurrentCultureIgnoreCase)
            );
        }

        OnUI(() => {
            Filtered = new ObservableCollection<Invoice>(result);
            ReSelect(previousSelection);
        });
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(i => i.Id == id) is Invoice selected)
            SelectedSummary = selected;
    }
}
