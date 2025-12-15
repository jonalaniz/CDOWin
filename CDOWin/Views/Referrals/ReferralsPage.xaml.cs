using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Referrals.Inspectors;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Referrals;

public sealed partial class ReferralsPage : Page {
    public ReferralsViewModel ViewModel { get; }

    public ReferralsPage() {
        InitializeComponent();
        ViewModel = AppServices.ReferralsViewModel;
        DataContext = ViewModel;
        InspectorFrame.Navigate(typeof(ReferralInspector), ViewModel);
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Referral referral) {
            ViewModel.RefreshSelectedReferral(referral.id);
        }
    }
}
