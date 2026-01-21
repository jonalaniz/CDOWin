using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.ServiceAuthorizations.Inspectors;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.ServiceAuthorizations;

public sealed partial class ServiceAuthorizationsPage : Page {

    // =========================
    // ViewModel
    // =========================
    public ServiceAuthorizationsViewModel ViewModel { get; } = AppServices.SAsViewModel;

    // =========================
    // Constructor
    // =========================
    public ServiceAuthorizationsPage() {
        InitializeComponent();
        InspectorFrame.Navigate(typeof(ServiceAuthorizationInspector));
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await ViewModel.LoadServiceAuthorizationsAsync();
    }

    // =========================
    // Click Handlers
    // =========================
    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Invoice sa) {
            _ = ViewModel.ReloadServiceAuthorizationAsync(sa.ServiceAuthorizationNumber);
        }
    }

    private void GoToClient_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        ViewModel.RequestClient(id);
    }
}
