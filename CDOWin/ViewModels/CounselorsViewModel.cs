using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CounselorsViewModel : ObservableObject {
    private readonly ICounselorService _service;

    [ObservableProperty]
    public partial ObservableCollection<Counselor> All { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Counselor> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Counselor? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    public CounselorsViewModel(ICounselorService service) {
        _service = service;
    }

    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    void ApplyFilter() {
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

    public async Task LoadCounselorsAsync() {
        var counselors = await _service.GetAllCounselorsAsync();
        List<Counselor> SortedCounselors = counselors.OrderBy(o => o.name).ToList();
        All.Clear();

        foreach (var counselor in SortedCounselors) {
            All.Add(counselor);
        }

        ApplyFilter();
    }

    public async Task RefreshSelectedCounselor(int id) {
        var counselor = await _service.GetCounselorAsync(id);
        var index = All.IndexOf(All.First(c => c.id == counselor.id));
        All[index] = counselor;
        Selected = counselor;
    }

    public async Task UpdateCounselor(UpdateCounselorDTO update) {
        if (Selected == null)
            return;
        var updatedCounselor = await _service.UpdateCounselorAsync(Selected.id, update);
        await RefreshSelectedCounselor(Selected.id);
        ApplyFilter();
        Selected = updatedCounselor;
    }
}
