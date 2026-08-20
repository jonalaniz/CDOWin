using CDO.Core.DTOs.Clients;
using CDO.UI.Shared.Factories;
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
    private readonly ClientsViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public ClientsPage() {
        _viewModel = AppServices.ClientsViewModel;
        InitializeComponent();
        ClientFrame.Navigate(typeof(ClientViewPage));
        InspectorFrame.Navigate(typeof(Notes));
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await _viewModel.RefreshAsync();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void NewClient_Click(object sender, RoutedEventArgs e) {
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, "New Client");
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

        await _viewModel.RefreshAsync(force: true);
        _viewModel.Selected = updateResult.Value;
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        var selection = (ClientSummary)e.ClickedItem;
        _ = _viewModel.LoadSelectedClientAsync(selection.Id);
    }
}
