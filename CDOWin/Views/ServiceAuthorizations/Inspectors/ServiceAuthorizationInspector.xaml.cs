using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.ServiceAuthorizations.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.ServiceAuthorizations.Inspectors;

public sealed partial class ServiceAuthorizationInspector : Page {

    // =========================
    // ViewModel
    // =========================
    public ServiceAuthorizationsViewModel ViewModel { get; } = AppServices.SAsViewModel;

    // =========================
    // Constructor
    // =========================
    public ServiceAuthorizationInspector() {
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null)
            return;

        var updateVM = new ServiceAuthorizationUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Service Authorization");
        dialog.Content = new UpdateSA(updateVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
            _ = ViewModel.UpdateSAAsync(updateVM.Updated);
    }
}
