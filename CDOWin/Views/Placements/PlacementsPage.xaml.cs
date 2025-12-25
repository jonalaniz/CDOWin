using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Placements.Inspectors;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Placements;

public sealed partial class PlacementsPage : Page {
    public PlacementsViewModel ViewModel { get; }

    public PlacementsPage() {
        InitializeComponent();
        ViewModel = AppServices.PlacementsViewModel;
        DataContext = ViewModel;
        InspectorFrame.Navigate(typeof(PlacementInspector), ViewModel);
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Placement placement) {
            ViewModel.RefreshSelectedPlacement(placement.id);
        }
    }
}
