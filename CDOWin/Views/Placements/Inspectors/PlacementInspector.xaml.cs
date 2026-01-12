using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Placements.Dialogs;
using CDOWin.Views.ServiceAuthorizations.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

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

    private async void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null) return;

        var updateVM = new PlacementUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Placement");
        dialog.Content = new UpdatePlacements(updateVM);

        var result = await dialog.ShowAsync();
    }
}
