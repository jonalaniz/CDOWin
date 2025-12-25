using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.Placements.Inspectors;

public sealed partial class PlacementInspector : Page {
    public PlacementsViewModel? ViewModel {
        get; private set;
    }
    public PlacementInspector() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (PlacementsViewModel)e.Parameter;
        DataContext = ViewModel;
    }
}
