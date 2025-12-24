using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class EmployersViewModel : ObservableObject {
    private readonly IEmployerService _service;

    [ObservableProperty]
    public partial ObservableCollection<Employer> All { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Employer> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Employer? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    public EmployersViewModel(IEmployerService service) {
        _service = service;
    }

    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Employer>(All);
            return;
        }

        var query = SearchQuery.Trim().ToLower();

        var result = All.Where(e =>
        (e.name?.ToLower().Contains(query) ?? false)
        || (e.formattedAddress?.ToLower().Contains(query) ?? false)
        || (e.email?.ToLower().Contains(query) ?? false)
        || (e.supervisor?.ToLower().Contains(query) ?? false)
        || (e.supervisorEmail?.ToLower().Contains(query) ?? false)
        || (e.notes?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<Employer>(result);
    }

    public async Task LoadEmployersAsync() {
        var employers = await _service.GetAllEmployersAsync();
        List<Employer> SortedEmployers = employers.OrderBy(o => o.name).ToList();
        All.Clear();

        foreach (var employer in SortedEmployers) {
            All.Add(employer);
        }

        ApplyFilter();
    }

    public async Task UpdateEmployer(EmployerDTO update) {
        if (Selected == null)
            return;
        var updatedEmployer = await _service.UpdateEmployerAsync(Selected.id, update);
        await RefreshSelectedEmployer(Selected.id);
    }

    public async Task RefreshSelectedEmployer(int id) {
        var employer = await _service.GetEmployerAsync(id);
        Replace(All, employer);
        Replace(Filtered, employer);

        Selected = employer;
    }

    // Utility Methods
    private void Replace(ObservableCollection<Employer> list, Employer updated) {
        var index = list.IndexOf(list.First(e => e.id == updated.id));
        if (index >= 0)
            list[index] = updated;
    }
}
