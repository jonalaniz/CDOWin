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

public partial class CounselorsViewModel : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly ICounselorService _service;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<Counselor> _allCounselors = [];

    // =========================
    // Public Property / State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<Counselor> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Counselor? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public CounselorsViewModel(ICounselorService service) {
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
    // Public Methods
    // =========================

    public List<Counselor> All() => _allCounselors.ToList();

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadCounselorsAsync() {
        var counselors = await _service.GetAllCounselorsAsync();
        if (counselors == null) return;

        var snapshot = counselors.OrderBy(o => o.name).ToList().AsReadOnly();
        _allCounselors = snapshot;

        _dispatcher.TryEnqueue(() => { 
            ApplyFilter();
        });
    }

    public async Task ReloadCounselorAsync(int id) {
        var counselor = await _service.GetCounselorAsync(id);
        if (counselor == null) return;

        var updated = _allCounselors
            .Select(c => c.id == id ? counselor : c)
            .ToList()
            .AsReadOnly();

        _allCounselors = updated;
        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
            Selected = counselor;
        });

        Selected = counselor;
    }

    public async Task UpdateCounselorAsync(UpdateCounselorDTO update) {
        if (Selected == null) return;
        var updatedCounselor = await _service.UpdateCounselorAsync(Selected.id, update);
        await ReloadCounselorAsync(Selected.id);
    }

    // =========================
    // Utility / Filtering
    // =========================

    private void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Counselor>(_allCounselors);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allCounselors.Where(c =>
        (c.name?.ToLower().Contains(query) ?? false)
        || (c.secretaryName?.ToLower().Contains(query) ?? false)
        || (c.email?.ToLower().Contains(query) ?? false)
        || (c.secretaryEmail?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<Counselor>(result);
    }
}