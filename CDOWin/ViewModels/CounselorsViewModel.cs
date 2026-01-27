using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
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

public partial class CounselorsViewModel : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly ICounselorService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientSelectionService _clientSelectionService;
    private readonly CounselorSelectionService _counselorSelectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<CounselorSummaryDTO> _allCounselors = [];

    // =========================
    // View State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<CounselorSummaryDTO> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial CounselorResponseDTO? Selected { get; set; }

    [ObservableProperty]
    public partial CounselorSummaryDTO? SelectedSummary { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<ClientSummaryDTO> Clients { get; private set; } = [];

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
    partial void OnSearchQueryChanged(string value) {
        if (_dispatcher.HasThreadAccess)
            ApplyFilter();
        else
            _dispatcher.TryEnqueue(ApplyFilter);
    }

    private void OnRequestSelectedCounselorChange(int counselorId) {
        if (Selected != null && Selected.Id == counselorId) return;
        SearchQuery = string.Empty;
        ApplyFilter();
        _ = LoadSelectedCounselorAsync(counselorId);
    }

    // =========================
    // Public Methods
    // =========================
    public List<CounselorSummaryDTO> All() => _allCounselors.ToList();

    public List<CounselorSummaryDTO> GetCounselors() {
        if (_allCounselors.Count == 0)
            LoadCounselorSummariesAsync().GetAwaiter().GetResult();

        return _allCounselors.ToList();
    }

    public void RequestClient(int clientID) {
        AppServices.Navigation.Navigate(Views.CDOFrame.Clients);
        _clientSelectionService.RequestSelectedClient(clientID);
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadCounselorSummariesAsync(bool force = false) {
        var counselors = await _dataCoordinator.GetCounselorsAsync(force);
        if (counselors == null) return;

        var snapshot = counselors.OrderBy(o => o.Name).ToList().AsReadOnly();
        _allCounselors = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task LoadSelectedCounselorAsync(int id) {
        if (Selected != null && Selected.Id == id) return;

        var counselor = await _service.GetCounselorAsync(id);
        Selected = counselor;

        _dispatcher.TryEnqueue(() => {
            Clients = new ObservableCollection<ClientSummaryDTO>(counselor.Clients);
        });
    }

    public async Task ReloadCounselorAsync() {
        if (Selected == null) return;
        Selected = await _service.GetCounselorAsync(Selected.Id);
    }

    public async Task<Result<Counselor>> UpdateCounselorAsync(UpdateCounselorDTO update) {
        if (Selected == null) return Result<Counselor>.Fail(new AppError(ErrorKind.Validation, "Counselor not selected.", null));

        var result = await _service.UpdateCounselorAsync(Selected.Id, update);
        if (!result.IsSuccess) return result;

        // TODO: Mark data as stale in DataCoordinator to fetch new data on next update

        await ReloadCounselorAsync();
        return result;
    }

    public async Task<Result<bool>> DeleteSelectedCounselor() {
        if (Selected == null) return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No Counselor Selected.", null, null));
        var id = Selected.Id;
        var result = await _service.DeleteCounselorAsync(id);

        if (result.IsSuccess) {
            _dispatcher.TryEnqueue(() => {
                Selected = null;
                SelectedSummary = null;
                _allCounselors = _allCounselors
                    .Where(c => c.Id != id)
                    .ToList()
                    .AsReadOnly();
                ApplyFilter();
            });

            _ = LoadCounselorSummariesAsync(force: true);
        }

        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================

    private void ApplyFilter() {
        int? previousSelection = Selected?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<CounselorSummaryDTO>(_allCounselors);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allCounselors.Where(c =>
        (c.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.CaseLoadID.ToString() ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.SecretaryName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.Email ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        Filtered = new ObservableCollection<CounselorSummaryDTO>(result);
        ReSelect(previousSelection);
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(c => c.Id == id) is CounselorSummaryDTO selected)
            SelectedSummary = selected;
    }
}