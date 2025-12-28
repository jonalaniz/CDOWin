using CDOWin.ViewModels;
using CDOWin.Views.Clients.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace CDOWin.Views.Clients.Inspectors;

public sealed partial class Notes : Page {
    public ClientsViewModel ViewModel { get; private set; } = null!;

    public Notes() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (ClientsViewModel)e.Parameter;
        DataContext = ViewModel;
    }

    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (ViewModel.SelectedClient == null) return;
        var updateVM = new ClientUpdateViewModel(ViewModel.SelectedClient);

        ContentDialog dialog = new();
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.PrimaryButtonText = "Add to Notes";
        dialog.CloseButtonText = "Cancel";
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Title = "Add New Note";
        dialog.Content = new UpdateNotes(updateVM);

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
            _ = ViewModel.UpdateClientAsync(updateVM.UpdatedClient);
    }
}
