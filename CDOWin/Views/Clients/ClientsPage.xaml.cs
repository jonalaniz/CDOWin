using CDO.Core.DTOs.Clients;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Dialogs;
using CDOWin.Views.Clients.Inspectors;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.ComponentModel;

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
    private async void NewClient_Click(object sender, RoutedEventArgs e) {
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, "New ClientDetail");
        var createClientVM = AppServices.CreateClientViewModel();
        var createClientPage = new CreateClient(createClientVM);
        dialog.Content = createClientPage;
        dialog.IsPrimaryButtonEnabled = createClientVM.CanSave;

        PropertyChangedEventHandler handler = (_, args) => {
            if (args.PropertyName == nameof(createClientVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createClientVM.CanSave;
        };

        createClientVM.PropertyChanged += handler;

        var result = await dialog.ShowAsync();
        createClientVM.PropertyChanged -= handler;

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await createClientVM.CreateClientAsync();
        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }

        await ViewModel.LoadClientSummariesAsync(force: true);
        ViewModel.Selected = updateResult.Value;
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        var selection = (ClientSummary)e.ClickedItem;
        _ = ViewModel.LoadSelectedClientAsync(selection.Id);
    }
}
