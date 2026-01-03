using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class EmployersViewModel : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly IEmployerService _service;
    private readonly DispatcherQueue _dispatcher;

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
    // Constructor
    // =========================
    public EmployersViewModel(IEmployerService service) {
        _service = service;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
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
    public async Task LoadEmployersAsync() {
        var employers = await _service.GetAllEmployersAsync();
        if (employers == null) return;

        var snapshot = employers.OrderBy(e => e.id).ToList().AsReadOnly();
        _allEmployers = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task ReloadEmployerAsync(int id) {
        var employer = await _service.GetEmployerAsync(id);
        if (employer == null) return;

        var updated = _allEmployers
            .Select(e => e.id == id ? employer : e)
            .ToList()
            .AsReadOnly();

        _allEmployers = updated;
        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
            Selected = employer;
        });

        Selected = employer;
    }

    public async Task UpdateEmployerAsync(EmployerDTO update) {
        if (Selected == null)
            return;
        var updatedEmployer = await _service.UpdateEmployerAsync(Selected.id, update);
        await ReloadEmployerAsync(Selected.id);
    }

    // =========================
    // Utility / Filtering
    // =========================

    private void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Employer>(_allEmployers);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allEmployers.Where(e =>
        (e.name?.ToLower().Contains(query) ?? false)
        || (e.formattedAddress?.ToLower().Contains(query) ?? false)
        || (e.email?.ToLower().Contains(query) ?? false)
        || (e.supervisor?.ToLower().Contains(query) ?? false)
        || (e.supervisorEmail?.ToLower().Contains(query) ?? false)
        || (e.notes?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<Employer>(result);
    }
}
