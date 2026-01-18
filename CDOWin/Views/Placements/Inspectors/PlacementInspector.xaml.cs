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
    public PlacementsViewModel ViewModel { get; } = AppServices.PlacementsViewModel;

    // =========================
    // Constructor
    // =========================
    public PlacementInspector() {
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void EditButton_Click(object sender, RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null) return;

        var updateVM = new PlacementUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Placement");
        dialog.Content = new UpdatePlacement(updateVM);

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await updateVM.UpdatePlacementAsync();

        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }

        _ = ViewModel.ReloadPlacementAsync(ViewModel.Selected.Id);
    }

    private async void Delete_Click(object sender, RoutedEventArgs e) {
        if (ViewModel.Selected == null) return;

        var dialog = DialogFactory.DeleteDialog(this.XamlRoot, "Delete Placement?");
        dialog.Content = new DeletePage();

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary) {
            await ViewModel.DeleteSelectedPlacement();
        }
    }
}
