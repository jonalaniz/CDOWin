using CDO.Core.DTOs;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Inspectors;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.Clients;

public sealed partial class ClientsPage : Page {

    // =========================
    // ViewModel
    // =========================
    public ClientsViewModel ViewModel { get; } = AppServices.ClientsViewModel;

    // =========================
    // Constructor
    // =========================
    public ClientsPage() {
        InitializeComponent();
        ClientFrame.Navigate(typeof(ClientViewPage));
        InspectorFrame.Navigate(typeof(Notes));
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await ViewModel.LoadClientSummariesAsync();
    }

    // =========================
    // Click Handlers
    // =========================
    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        var selection = (ClientSummaryDTO)e.ClickedItem;
        _ = ViewModel.LoadSelectedClientAsync(selection.Id);
    }
}
