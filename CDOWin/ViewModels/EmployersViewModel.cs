using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class EmployersViewModel(DataCoordinator dataCoordinator, IEmployerService service) : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly IEmployerService _service = service;
    private readonly DataCoordinator _dataCoordinator = dataCoordinator;
    private readonly DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<Employer> _allEmployers = [];

    // =========================
    // Public Property / State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<Employer> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Employer? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

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
    public async Task LoadEmployersAsync() {
        var employers = await _dataCoordinator.GetEmployersAsync();
        if (employers == null) return;

        var snapshot = employers.OrderBy(e => e.Id).ToList().AsReadOnly();
        _allEmployers = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task ReloadEmployerAsync(int id) {
        var employer = await _service.GetEmployerAsync(id);
        if (employer == null) return;

        var updated = _allEmployers
            .Select(e => e.Id == id ? employer : e)
            .ToList()
            .AsReadOnly();

        _allEmployers = updated;

        _dispatcher.TryEnqueue(() => {
            var index = Filtered
            .Select((e, i) => new { e, i })
            .FirstOrDefault(x => x.e.Id == id)?.i;

            if (index != null)
                Filtered[index.Value] = employer;

            Selected = employer;
        });
    }

    public async Task UpdateEmployerAsync(EmployerDTO update) {
        if (Selected == null)
            return;
        _ = await _service.UpdateEmployerAsync(Selected.Id, update);
        await ReloadEmployerAsync(Selected.Id);
    }

    // =========================
    // Utility / Filtering
    // =========================

    private void ApplyFilter() {
        int? previousSelection = Selected?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Employer>(_allEmployers);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allEmployers.Where(e =>
        (e.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (e.FormattedAddress ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (e.Email ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (e.Supervisor ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (e.SupervisorEmail ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (e.Notes ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        Filtered = new ObservableCollection<Employer>(result);
        ReSelect(previousSelection);
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(e => e.Id == id) is Employer selected)
            Selected = selected;
    }
}
