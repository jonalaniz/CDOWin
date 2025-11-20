using CDO.Core.Models;
using CDO.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CounselorsViewModel : ObservableObject {
    private readonly ICounselorService _service;

    [ObservableProperty]
    public partial ObservableCollection<Counselor> Counselors { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Counselor> FilteredCounselors { get; private set; } = [];

    [ObservableProperty]
    public partial Counselor? SelectedCounselor { get; set; }

    public CounselorsViewModel(ICounselorService service) {
        _service = service;
    }

    partial void OnSelectedCounselorChanged(Counselor? value) {
        if (value != null)
        _ = RefreshSelectedCounselor(value.id);
    }

    public async Task LoadCounselorAsync() {
        var counselors = await _service.GetAllCounselorsAsync();
        List<Counselor> SortedCounselors = counselors.OrderBy(o => o.name).ToList();
        Counselors.Clear();

        foreach (var counselor in SortedCounselors) {
            Counselors.Add(counselor);
        }
    }

    public async Task RefreshSelectedCounselor(int id) {
        var counselor = await _service.GetCounselorAsync(id);
        if (SelectedCounselor != counselor) {
            SelectedCounselor = counselor;

            var index = Counselors.IndexOf(Counselors.First(c => c.id == id));
            Counselors[index] = counselor;
        }
    }
}
