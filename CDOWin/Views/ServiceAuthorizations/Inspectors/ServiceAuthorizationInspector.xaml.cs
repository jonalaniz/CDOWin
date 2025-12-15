using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.ServiceAuthorizations.Inspectors;

public sealed partial class ServiceAuthorizationInspector : Page {
    public ServiceAuthorizationsViewModel? ViewModel { get; set; }

    public ServiceAuthorizationInspector() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (ServiceAuthorizationsViewModel)e.Parameter;
        DataContext = ViewModel;
    }
}
