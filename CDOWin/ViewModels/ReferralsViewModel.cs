using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ReferralsViewModel : ObservableObject {
    private readonly IReferralService _service;

    [ObservableProperty]
    public partial ObservableCollection<Referral> All { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Referral> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Referral? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    public ReferralsViewModel(IReferralService service) {
        _service = service;
    }

    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Referral>(All);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = All.Where(r =>
        (r.clientName?.ToLower().Contains(query) ?? false)
        || (r.employer.name?.ToLower().Contains(query) ?? false)
        || (r.supervisor?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<Referral>(result);
    }

    public async Task LoadReferralsAsync() {
        var referrals = await _service.GetAllReferralsAsync();
        List<Referral> SortedReferrals = referrals.OrderBy(o => o.clientID).ToList();
        All.Clear();

        foreach (var referral in SortedReferrals) {
            All.Add(referral);
        }

        ApplyFilter();
    }

    public async Task RefreshSelectedReferral(string id) {
        var referral = await _service.GetReferralAsync(id);
        var index = All.IndexOf(All.First(r => r.id == referral.id));
        All[index] = referral;
        Selected = referral;
    }
}
