using CDO.Core.DTOs;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Inspectors;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Clients;

public sealed partial class ClientsPage : Page {
    public readonly ClientsViewModel ViewModel;

    public ClientsPage() {
        InitializeComponent();
        ViewModel = AppServices.ClientsViewModel;
        DataContext = ViewModel;
        ClientFrame.Navigate(typeof(ClientViewPage), ViewModel);
        InspectorFrame.Navigate(typeof(Notes), ViewModel);
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        var selection = (ClientSummaryDTO)e.ClickedItem;
        _ = ViewModel.LoadSelectedClientAsync(selection.Id);
    }
}
