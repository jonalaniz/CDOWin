using CDO.Core.DTOs.SAs;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.ServiceAuthorizations.Inspectors;
using Microsoft.UI.Xaml;
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
        if (e.ClickedItem is SASummary sa) {
            _ = ViewModel.LoadSelectedSAAsync(sa.Id);
        }
    }

    private void GoToClient_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        ViewModel.RequestClient(id);
    }

    private void GoToCounselor_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        ViewModel.RequestCounselor(id);
    }

    private async void ToggleSort_Click(object sender, RoutedEventArgs e) {
        if (sender is not AppBarButton button) return;
        button.IsEnabled = false;
        await ViewModel.ToggleSortAsync();
        button.IsEnabled = true;
    }
}
