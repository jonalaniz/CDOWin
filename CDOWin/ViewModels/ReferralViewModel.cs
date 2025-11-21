using CDO.Core.Models;
using CDO.Core.Services;
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

    public ReferralsViewModel(IReferralService service) {
        _service = service;
    }

    partial void OnSelectedReferralChanged(Referral? value) {
        if (value != null)
            _ = RefreshSelectedReferral(value.id);
    }

    public async Task LoadPOAsync() {
        var referrals = await _service.GetAllReferralsAsync();
        List<Referral> SortedReferrals = referrals.OrderBy(o => o.clientID).ToList();
        Referrals.Clear();

        foreach (var referral in SortedReferrals) {
            Referrals.Add(referral);
        }
    }

    public async Task RefreshSelectedReferral(string id) {
        var referral = await _service.GetReferralAsync(id);
        if (SelectedReferral != referral) {
            SelectedReferral = referral;

            var index = Referrals.IndexOf(Referrals.First(r => r.id == id));
            Referrals[index] = referral;
        }
    }
}
