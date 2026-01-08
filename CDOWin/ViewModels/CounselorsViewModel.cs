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

public partial class CounselorsViewModel(DataCoordinator dataCoordinator, ICounselorService service) : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly ICounselorService _service = service;
    private readonly DataCoordinator _dataCoordinator = dataCoordinator;
    private readonly DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();

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
        var counselors = await _dataCoordinator.GetCounselorsAsync();
        if (counselors == null) return;

        var snapshot = counselors.OrderBy(o => o.Name).ToList().AsReadOnly();
        _allCounselors = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task ReloadCounselorAsync(int id) {
        var counselor = await _service.GetCounselorAsync(id);
        if (counselor == null) return;

        var updated = _allCounselors
            .Select(c => c.Id == id ? counselor : c)
            .ToList()
            .AsReadOnly();

        _allCounselors = updated;

        _dispatcher.TryEnqueue(() => {
            var index = Filtered
            .Select((c, i) => new { c, i })
            .FirstOrDefault(x => x.c.Id == id)?.i;

            if (index != null)
                Filtered[index.Value] = counselor;

            Selected = counselor;
        });
    }

    public async Task UpdateCounselorAsync(UpdateCounselorDTO update) {
        if (Selected == null) return;
        _ = await _service.UpdateCounselorAsync(Selected.Id, update);
        await ReloadCounselorAsync(Selected.Id);
    }

    // =========================
    // Utility / Filtering
    // =========================

    private void ApplyFilter() {
        int? previousSelection = Selected?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Counselor>(_allCounselors);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allCounselors.Where(c =>
        (c.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.SecretaryName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.Email ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.SecretaryEmail ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        Filtered = new ObservableCollection<Counselor>(result);
        ReSelect(previousSelection);
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(c => c.Id == id) is Counselor selected)
            Selected = selected;
    }
}