using CDO.Core.DTOs.Employers;
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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class EmployersViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IEmployerService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly EmployerSelectionService _selectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<EmployerSummary> _cache = [];

    // =========================
    // UI State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<EmployerSummary> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Employer? Selected { get; set; }

    [ObservableProperty]
    public partial EmployerSummary? SelectedSummary { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public EmployersViewModel(
        DataCoordinator dataCoordinator,
        IEmployerService service,
        EmployerSelectionService selectionService) {
        _service = service;
        _dataCoordinator = dataCoordinator;

        _selectionService = selectionService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _selectionService.EmployerSelectionRequested += OnRequestSelectedEmployerChanged;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) => ApplyFilter();

    private void OnRequestSelectedEmployerChanged(int employerId) {
        if (Selected != null && Selected.Id == employerId) return;
        SearchQuery = string.Empty;
        ApplyFilter();
        _ = LoadSelectedEmployerAsync(employerId);
    }

    // =========================
    // Public Methods
    // =========================
    public async Task<List<Employer>> GetEmployers() {
        var employers = await _service.GetAllEmployersAsync();
        return employers == null ? new List<Employer>() : employers.ToList();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadEmployerSummariesAsync(bool force = false) {
        var employers = await _dataCoordinator.GetEmployerSummariesAsync(force);
        if (employers == null) return;

        var snapshot = employers.OrderBy(e => e.Name).ToList().AsReadOnly();
        _cache = snapshot;
        ApplyFilter();
    }

    public async Task LoadSelectedEmployerAsync(int id) {
        if (Selected != null && Selected.Id == id) return;
        Debug.WriteLine("we up in this hoe");

        var selectedEmployer = await _service.GetEmployerAsync(id);
        Selected = selectedEmployer;
    }

    public async Task ReloadEmployerAsync() {
        if (Selected == null) return;
        Selected = await _service.GetEmployerAsync(Selected.Id);
    }

    public async Task<Result> UpdateEmployerAsync(EmployerDTO update) {
        if (SelectedSummary == null) return Result<Employer>.Fail(new AppError(ErrorKind.Validation, "EmployerName not selected.", null));

        var result = await _service.UpdateEmployerAsync(SelectedSummary.Id, update);
        if (result.IsSuccess) { await ReloadEmployerAsync(); }
        return result;
    }

    public async Task<Result> DeleteSelectedEmployer() {
        if (Selected == null)
            return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No EmployerName Selected.", null, null));

        var id = Selected.Id;
        var result = await _service.DeleteEmployerAsync(id);

        if (result.IsSuccess) {
            OnUI(() => {
                Selected = null;
                SelectedSummary = null;
            });
            _ = LoadEmployerSummariesAsync(force: true);
        }

        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================

    private void ApplyFilter() {
        int? previousSelection = SelectedSummary?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<EmployerSummary>(_cache);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _cache.Where(e =>
        (e.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (e.FormattedAddress ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (e.Supervisor ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (e.Notes ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        OnUI(() => {
            Filtered = new ObservableCollection<EmployerSummary>(result);
            ReSelect(previousSelection);
        });
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(e => e.Id == id) is EmployerSummary selected)
            SelectedSummary = selected;
    }
}
