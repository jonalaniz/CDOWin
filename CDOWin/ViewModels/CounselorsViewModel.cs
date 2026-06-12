using CDO.Core.DTOs.Clients;
using CDO.Core.DTOs.Counselors;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDOWin.Data;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CounselorsViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly ICounselorService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientSelectionService _clientSelectionService;
    private readonly CounselorSelectionService _counselorSelectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private CancellationTokenSource? _filterCts;

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<CounselorSummary> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial CounselorDetail? Selected { get; set; }

    [ObservableProperty]
    public partial CounselorSummary? SelectedSummary { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<ClientSummary> Clients { get; private set; } = [];

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public CounselorsViewModel(
        DataCoordinator dataCoordinator,
        ICounselorService service,
        CounselorSelectionService counselorSelectionService,
        ClientSelectionService clientSelectionService) {
        _service = service;
        _dataCoordinator = dataCoordinator;

        _clientSelectionService = clientSelectionService;
        _counselorSelectionService = counselorSelectionService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _counselorSelectionService.CounselorSelectionRequested += OnRequestSelectedCounselorChange;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) => _ = RefreshAsync();

    private void OnRequestSelectedCounselorChange(int counselorId) {
        if (Selected != null && Selected.Id == counselorId) return;
        SearchQuery = string.Empty;
        _ = LoadSelectedCounselorAsync(counselorId);
    }

    // =========================
    // Public Methods
    // =========================

    // Get method for CreateCounselor.xaml.cs
    public async Task<List<CounselorSummary>> GetCounselors() {
        var counselors = await _dataCoordinator.GetCounselorsAsync();
        return counselors.ToList();
    }

    public void RequestClient(int clientID) {
        AppServices.Navigation.Navigate(Views.CDOFrame.Clients);
        _clientSelectionService.RequestSelectedClient(clientID);
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadSelectedCounselorAsync(int id) {
        if (Selected != null && Selected.Id == id) return;

        var counselor = await _service.GetCounselorAsync(id);

        OnUI(() => {
            Selected = counselor;
            Clients = new ObservableCollection<ClientSummary>(counselor?.Clients ?? []);
        });
    }

    public async Task ReloadCounselorAsync() {
        if (Selected == null) return;
        Selected = await _service.GetCounselorAsync(Selected.Id);
    }

    public async Task<Result> UpdateCounselorAsync(CounselorUpdate update) {
        if (Selected == null) return Result.Fail(new AppError(ErrorKind.Validation, "Counselor not selected.", null));

        var result = await _service.UpdateCounselorAsync(Selected.Id, update);
        if (result.IsSuccess) { await ReloadCounselorAsync(); }
        return result;
    }

    public async Task<Result> DeleteSelectedCounselor() {
        if (Selected == null)
            return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No Counselor Selected.", null, null));

        var id = Selected.Id;
        var result = await _service.DeleteCounselorAsync(id);

        if (result.IsSuccess) OnUI(() => RemoveDeletedCounselor(id));

        return result;
    }

    // =========================
    // Utility / Helpers
    // =========================
    public async Task RefreshAsync(bool force = false) {
        _filterCts?.Cancel();
        _filterCts = new CancellationTokenSource();
        var token = _filterCts.Token;

        try {
            await Task.Delay(150, token);
            if (token.IsCancellationRequested) return;

            var snapshot = await _dataCoordinator.GetCounselorsAsync(force);
            if (token.IsCancellationRequested) return;

            int? previousSelection = Selected?.Id;

            if (string.IsNullOrWhiteSpace(SearchQuery)) {
                OnUI(() => {
                    Filtered = new ObservableCollection<CounselorSummary>(
                        snapshot.OrderBy(s => s.Name)
                        );
                    ReSelect(previousSelection);
                });
                return;
            }

            var query = SearchQuery.Trim().ToLower();
            var result = snapshot.Where(c =>
            (c.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (c.CaseLoadID.ToString() ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (c.SecretaryName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
            (c.Email ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
            );

            OnUI(() => {
                Filtered = new ObservableCollection<CounselorSummary>(
                    result.OrderBy(s => s.Name)
                    );
                ReSelect(previousSelection);
            });
        } catch (OperationCanceledException) { }
    }

    private void RemoveDeletedCounselor(int id) {
        // Update our cache
        _ = _dataCoordinator.GetCounselorsAsync(force: true);

        if (Filtered.FirstOrDefault(c => c.Id == id) is CounselorSummary counselor) {
            Filtered.Remove(counselor);
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
        if (Filtered.FirstOrDefault(c => c.Id == id) is CounselorSummary selected)
            SelectedSummary = selected;
    }
}