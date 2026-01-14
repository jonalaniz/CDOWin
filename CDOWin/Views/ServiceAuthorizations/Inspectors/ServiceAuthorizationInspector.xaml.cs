using CDO.Core.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.ServiceAuthorizations.Dialogs;
using Microsoft.UI.Xaml;
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
    private async void EditButton_Click(object sender, RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null) return;

        var updateVM = new ServiceAuthorizationUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Service Authorization");
        dialog.Content = new UpdateSA(updateVM);

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await updateVM.UpdateSAAsync();

        if (!updateResult.IsSuccess) {
            HandleErrorAsync(updateResult);
            return;
        }

        _ = ViewModel.ReloadServiceAuthorizationAsync(ViewModel.Selected.Id);
    }

    private async void Export_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (ViewModel.Selected == null) return;

        var result = await ViewModel.ExportSelectedAsync();
        if (!result.IsSuccess) {
            HandleErrorAsync(result);
            return;
        }
    }

    private async void HandleErrorAsync(Result result) {
        if (result.Error is not AppError error) return;
        var message = $"{error.Exception.Source}: {error.Exception.InnerException}: {error.Exception.StackTrace}";
        var dialog = DialogFactory.ErrorDialog(this.XamlRoot, error.Kind.ToString(), error.Exception.Message);
        await dialog.ShowAsync();
    }
}
