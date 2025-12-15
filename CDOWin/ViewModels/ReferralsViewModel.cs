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
    public partial ObservableCollection<Referral> Referrals { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Referral> FilteredReferrals { get; private set; } = [];

    [ObservableProperty]
    public partial Referral? SelectedReferral { get; set; }

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
            FilteredReferrals = new ObservableCollection<Referral>(Referrals);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = Referrals.Where(r =>
        (r.clientName?.ToLower().Contains(query) ?? false)
        || (r.employer.name?.ToLower().Contains(query) ?? false)
        || (r.supervisor?.ToLower().Contains(query) ?? false)
        );

        FilteredReferrals = new ObservableCollection<Referral>(result);
    }

    public async Task LoadReferralsAsync() {
        var referrals = await _service.GetAllReferralsAsync();
        List<Referral> SortedReferrals = referrals.OrderBy(o => o.clientID).ToList();
        Referrals.Clear();

        foreach (var referral in SortedReferrals) {
            Referrals.Add(referral);
        }

        ApplyFilter();
    }

    public async Task RefreshSelectedReferral(string id) {
        var referral = await _service.GetReferralAsync(id);
        var index = Referrals.IndexOf(Referrals.First(r => r.id == referral.id));
        Referrals[index] = referral;
        SelectedReferral = referral;
    }
}
