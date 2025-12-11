using CDO.Core.DTOs;
using CDOWin.ViewModels;
using CDOWin.Views.Inspectors;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace CDOWin.Views;

public sealed partial class ClientsPage : Page {
    public readonly ClientsViewModel ViewModel;

    public ClientsPage() {
        InitializeComponent();
        ViewModel = AppServices.ClientsViewModel;
        DataContext = ViewModel;
        ClientFrame.Navigate(typeof(ClientViewPage), ViewModel);
    }

    private void SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args) {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);
        System.Type pageType;

        switch (currentSelectedIndex) {
            case 0:
                pageType = typeof(Notes);
                break;
            case 1:
                pageType = typeof(Placements);
                break;
            default:
                pageType = typeof(SamplePage);
                break;
        }

        var slideNavigationTransitionEffect = SlideNavigationTransitionEffect.FromBottom;

        ContentFrame.Navigate(pageType, ViewModel, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        var selection = (ClientSummaryDTO)e.ClickedItem;
        ViewModel.ClientSelected(selection.id);
    }
}
