using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.ServiceAuthorizations.Inspectors;
using Microsoft.UI.Xaml.Controls;

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
    // Click Handlers
    // =========================
    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is ServiceAuthorization sa) {
            _ = ViewModel.ReloadServiceAuthorizationAsync(sa.Id);
        }
    }
}
