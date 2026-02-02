using CDO.Core.DTOs;
using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Placements.Inspectors;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

namespace CDOWin.Views.Placements;

public sealed partial class PlacementsPage : Page {

    // =========================
    // ViewModel
    // =========================
    public PlacementsViewModel ViewModel { get; } = AppServices.PlacementsViewModel;

    // =========================
    // Constructor
    // =========================
    public PlacementsPage() {
        InitializeComponent();
        InspectorFrame.Navigate(typeof(PlacementInspector), ViewModel);
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await ViewModel.LoadPlacementSummariesAsync();
    }

    // =========================
    // Click Handlers
    // =========================
    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is PlacementSummaryDTO placement)
            _ = ViewModel.LoadSelectedPlacementAsync(placement.Id);
    }

    private void Filter_Click(object sender, RoutedEventArgs e) {
        if (sender is not AppBarToggleButton button) return;
        // Filter this shit
        Debug.WriteLine(button.IsChecked);
    }
}
