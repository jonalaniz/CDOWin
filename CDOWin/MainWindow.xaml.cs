using CDOWin.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CDOWin;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
    int previousSelectedIndex = 0;

    public MainWindow() {
        InitializeComponent();
        SetupWindow();
    }

    private void SetupWindow() {
        // Setup our fulscreen window
        ExtendsContentIntoTitleBar = true;

        // Setup Title bar
        var uiSettings = new Windows.UI.ViewManagement.UISettings();
        var accentColor = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Accent);
        AppWindow.TitleBar.ButtonForegroundColor = accentColor;

        // Set sizing and center the Window
        var manager = WindowManager.Get(this);
        manager.MinHeight = 800;
        manager.MinWidth = 1200;

        SidebarFrame.Navigate(typeof(RemindersPage));
    }

    private void NavigationLoaded(object sender, RoutedEventArgs e) {
        NavigationBar.SelectedItem = NavigationBar.MenuItems[0];
    }

    private void NavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        var selectedItem = (NavigationViewItem)args.SelectedItem;
        var currentSelectedIndex = sender.MenuItems.IndexOf(selectedItem);
        System.Type pageType;

        switch (currentSelectedIndex) {
            case 0:
                pageType = typeof(ClientsPage);
                break;
            case 1:
                pageType = typeof(CounselorsPage);
                break;
            case 2:
                pageType = typeof(EmployersPage);
                break;
            case 3:
                pageType = typeof(POsPage);
                break;
            default:
                pageType = typeof(SamplePage);
                break;
        }

        var slideNavigationTransitionEffect = currentSelectedIndex - previousSelectedIndex > 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;

        ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });

        previousSelectedIndex = currentSelectedIndex;
    }
}
