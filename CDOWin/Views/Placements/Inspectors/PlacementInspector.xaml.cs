using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

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
}
