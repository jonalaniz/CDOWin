using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
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

    // =========================
    // View State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<Counselor> All { get; private set; } = [];

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
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadCounselorsAsync() {
        var counselors = await _service.GetAllCounselorsAsync();
        if (counselors == null) return;

        List<Counselor> SortedCounselors = counselors.OrderBy(o => o.name).ToList();
        All.Clear();
        foreach (var counselor in SortedCounselors) {
            All.Add(counselor);
        }

        ApplyFilter();
    }

    public async Task ReloadCounselorAsync(int id) {
        var counselor = await _service.GetCounselorAsync(id);
        if (counselor == null) return;
        Replace(All, counselor);
        Replace(Filtered, counselor);

        Selected = counselor;
    }

    public async Task UpdateCounselorAsync(UpdateCounselorDTO update) {
        if (Selected == null)
            return;
        var updatedCounselor = await _service.UpdateCounselorAsync(Selected.id, update);
        await ReloadCounselorAsync(Selected.id);
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void Replace(ObservableCollection<Counselor> list, Counselor updated) {
        var index = list.IndexOf(list.First(c => c.id == updated.id));
        if (index >= 0)
            list[index] = updated;
    }

    private void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Counselor>(All);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = All.Where(c =>
        (c.name?.ToLower().Contains(query) ?? false)
        || (c.secretaryName?.ToLower().Contains(query) ?? false)
        || (c.email?.ToLower().Contains(query) ?? false)
        || (c.secretaryEmail?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<Counselor>(result);
    }
}