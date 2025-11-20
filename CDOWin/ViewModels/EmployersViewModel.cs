using CDO.Core.Constants;
using CDO.Core.Models;
using CDO.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;
public partial class EmployersViewModel : ObservableObject {
    private readonly IEmployerService _service;

    [ObservableProperty]
    public partial ObservableCollection<Employer> Employers { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Employer> FilteredEmployers { get; private set; } = [];

    [ObservableProperty]
    public partial Employer? SelectedEmployer { get; set; }

    public EmployersViewModel(IEmployerService service) {
        _service = service;
    }

    partial void OnSelectedEmployerChanged(Employer? value) {
        _ = RefreshSelectedEmployer(value.id);
    }

    public async Task LoadEmployersAsync() {
        var employers = await _service.GetAllEmployersAsync();
        List<Employer> SortedEmployers = employers.OrderBy(o => o.name).ToList();
        Employers.Clear();

        foreach (var employer in SortedEmployers) {
            Employers.Add(employer);
        }
    }

    public async Task RefreshSelectedEmployer(int id) {
        var employer = await _service.GetEmployerAsync(id);

        if(SelectedEmployer != employer) {
            SelectedEmployer = employer;

            var index = Employers.IndexOf(Employers.First(e => e.id == id));
            Employers[index] = employer;
        }
    }
}
