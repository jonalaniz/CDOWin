using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CounselorsViewModel : ObservableObject {
    private readonly ICounselorService _service;

    [ObservableProperty]
    public partial ObservableCollection<Counselor> AllCounselors { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Counselor> FilteredCounselors { get; private set; } = [];

    [ObservableProperty]
    public partial Counselor? SelectedCounselor { get; set; }

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
            FilteredCounselors = new ObservableCollection<Counselor>(AllCounselors);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = AllCounselors.Where(c =>
        (c.name?.ToLower().Contains(query) ?? false)
        || (c.secretaryName?.ToLower().Contains(query) ?? false)
        || (c.email?.ToLower().Contains(query) ?? false)
        || (c.secretaryEmail?.ToLower().Contains(query) ?? false)
        );

        FilteredCounselors = new ObservableCollection<Counselor>(result);
    }

    public async Task LoadCounselorsAsync() {
        var counselors = await _service.GetAllCounselorsAsync();
        List<Counselor> SortedCounselors = counselors.OrderBy(o => o.name).ToList();
        AllCounselors.Clear();

        foreach (var counselor in SortedCounselors) {
            AllCounselors.Add(counselor);
        }

        ApplyFilter();
        // FilteredCounselors = new ObservableCollection<Counselor>(AllCounselors);
    }

    public async Task RefreshSelectedCounselor(int id) {
        var counselor = await _service.GetCounselorAsync(id);
        var index = AllCounselors.IndexOf(AllCounselors.First(c => c.id == id));
        AllCounselors[index] = counselor;
        SelectedCounselor = counselor;
    }
}
