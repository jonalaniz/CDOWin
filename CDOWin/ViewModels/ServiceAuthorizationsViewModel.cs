using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDOWin.Composers;
using CDOWin.Data;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
    private CancellationTokenSource? _filterCts;

    private bool Reversed { get; set; } = true;

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<SASummary> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial SASummary? SelectedSummary { get; set; }

    [ObservableProperty]
    public partial SADetail? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsFiltered { get; set; } = true;

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
    partial void OnSearchQueryChanged(string value) => _ = RefreshAsync();
    partial void OnIsFilteredChanged(bool value) => _ = RefreshAsync();

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

    public async Task ToggleSortAsync() {
        Reversed = !Reversed;
        await RefreshAsync();
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

        var composer = new ServiceAuthorizationComposer(Selected);
        return composer.Compose();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadSelectedSAAsync(int id) {
        if (Selected != null && Selected.Id == id) return;

        var selectedSA = await _service.GetServiceAuthorizationAsync(id);
        Selected = selectedSA;
    }

    public async Task<Result> DeleteSelectedSA() {
        if (SelectedSummary == null) return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No SA selected.", null, null));
        var id = SelectedSummary.Id;
        var result = await _service.DeleteServiceAuthorizationAsync(id);

        if (result.IsSuccess) OnUI(() => RemoveDeletedSA(id));

        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================
    public async Task RefreshAsync(bool force = false) {
        _filterCts?.Cancel();
        _filterCts = new CancellationTokenSource();
        var token = _filterCts.Token;

        try {
            await Task.Delay(150, token);
            if (token.IsCancellationRequested) return;

            var snapshot = await _dataCoordinator.GetSAsAsync(force);
            if (token.IsCancellationRequested) return;

            int? previousSelection = SelectedSummary?.Id;

            snapshot = IsFiltered ? snapshot.Where(r => r.Active).ToList() : snapshot;

            snapshot = snapshot.OrderBy(o => o.EndDate).ToList();
            if (Reversed) snapshot = snapshot.Reverse().ToList();

            if (!string.IsNullOrWhiteSpace(SearchQuery)) {
                var query = SearchQuery.Trim().ToLower();
                snapshot = snapshot.Where(i =>
                i.ClientName.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                i.CounselorName.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                i.ServiceAuthorizationNumber.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                i.Description.Contains(query, StringComparison.CurrentCultureIgnoreCase)
                ).ToList();
            }

            OnUI(() => {
                Filtered = new ObservableCollection<SASummary>(snapshot);
                ReSelect(previousSelection);
            });
        } catch (OperationCanceledException) { }
    }

    private void RemoveDeletedSA(int id) {
        // Update our cache
        _ = _dataCoordinator.GetSAsAsync(force: true);

        if (Filtered.FirstOrDefault(s => s.Id == id) is SASummary sa) {
            Filtered.Remove(sa);
            Selected = null;
            SelectedSummary = null;
        }
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(i => i.Id == id) is SASummary selected)
            SelectedSummary = selected;
    }
}
