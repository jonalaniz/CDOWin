using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Placements.Dialogs;
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

        // THERE IS NO PLLACEMENTS UPDATE VM
        var updateVM = new PlacementUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Placement");
        dialog.Content = new UpdatePlacement(updateVM);

        var result = await dialog.ShowAsync();
    }


}
