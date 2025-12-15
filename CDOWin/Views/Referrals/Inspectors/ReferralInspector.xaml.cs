using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.Referrals.Inspectors;

public sealed partial class ReferralInspector : Page {
    public ReferralsViewModel? ViewModel { get; private set; 
    }
    public ReferralInspector() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (ReferralsViewModel)e.Parameter;
        DataContext = ViewModel;
    }
}
