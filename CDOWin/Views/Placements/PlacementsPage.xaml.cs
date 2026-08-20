using CDO.Core.DTOs.Placements;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Placements.Inspectors;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.Placements;

public sealed partial class PlacementsPage : Page {

    // =========================
    // ViewModel
    // =========================
    private readonly PlacementsViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public PlacementsPage() {
        _viewModel = AppServices.PlacementsViewModel;
        InitializeComponent();
        InspectorFrame.Navigate(typeof(PlacementInspector), _viewModel);
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await _viewModel.RefreshAsync();
        _ = _viewModel.ReloadPlacementAsync();
    }

    // =========================
    // Click Handlers
    // =========================
    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is PlacementSummary placement)
            _ = _viewModel.LoadSelectedPlacementAsync(placement.Id);
    }

    private void GoToClient_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        _viewModel.RequestClient(id);
    }

    private void GoToCounselor_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        _viewModel.RequestCounselor(id);
    }

    private void GoToEmployer_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        _viewModel.RequestEmployer(id);
    }

    private async void ToggleSort_Click(object sender, RoutedEventArgs e) {
        if (sender is not AppBarButton button) return;
        button.IsEnabled = false;
        await _viewModel.ToggleSortAsync();
        button.IsEnabled = true;
    }
}
