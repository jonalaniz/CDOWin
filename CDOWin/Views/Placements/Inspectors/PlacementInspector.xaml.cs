using CDO.UI.Shared.Factories;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Placements.Dialogs;
using CDOWin.Views.Shared.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.Placements.Inspectors;

public sealed partial class PlacementInspector : Page {

    // =========================
    // ViewModel
    // =========================
    private readonly PlacementsViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public PlacementInspector() {
        _viewModel = AppServices.PlacementsViewModel;
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void EditButton_Click(object sender, RoutedEventArgs e) {
        if (_viewModel == null || _viewModel.Selected == null) return;

        var updateVM = new PlacementUpdateViewModel(_viewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Placement");
        dialog.Content = new UpdatePlacement(updateVM);

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await updateVM.UpdatePlacementAsync();

        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }

        _ = _viewModel.ReloadPlacementAsync();
    }

    private async void Delete_Click(object sender, RoutedEventArgs e) {
        if (_viewModel.Selected == null) return;

        var dialog = DialogFactory.DeleteDialog(this.XamlRoot, "Delete Placement?");
        dialog.Content = new DeletePage();

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary) {
            await _viewModel.DeleteSelectedPlacement();
        }
    }
}
