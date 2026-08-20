using CDO.UI.Shared.Factories;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.ServiceAuthorizations.Dialogs;
using CDOWin.Views.Shared.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.ServiceAuthorizations.Inspectors;

public sealed partial class ServiceAuthorizationInspector : Page {

    // =========================
    // ViewModel
    // =========================
    private readonly ServiceAuthorizationsViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public ServiceAuthorizationInspector() {
        _viewModel = AppServices.SAsViewModel;
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void EditButton_Click(object sender, RoutedEventArgs e) {
        if (_viewModel == null || _viewModel.SelectedSummary == null || _viewModel.Selected == null) return;

        var updateVM = new ServiceAuthorizationUpdateViewModel(_viewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Service Authorization");
        dialog.Content = new UpdateSA(updateVM);

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await updateVM.UpdateSAAsync();

        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }

        _ = _viewModel.LoadSelectedSAAsync(_viewModel.SelectedSummary.Id);
        _ = _viewModel.RefreshAsync(force: true);
    }

    private async void Export_Click(object sender, RoutedEventArgs e) {
        if (_viewModel.Selected == null) return;

        var result = await _viewModel.ExportSelectedAsync();
        if (!result.IsSuccess) ErrorHandler.Handle(result, this.XamlRoot);
    }

    private async void Delete_Click(object sender, RoutedEventArgs e) {
        if (_viewModel.SelectedSummary == null) return;

        var dialog = DialogFactory.DeleteDialog(this.XamlRoot, "Delete Placement?");
        dialog.Content = new DeletePage();

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary) {
            var deleteResult = await _viewModel.DeleteSelectedSA();
            if (!deleteResult.IsSuccess)
                ErrorHandler.Handle(deleteResult, this.XamlRoot);
        }
    }
}
