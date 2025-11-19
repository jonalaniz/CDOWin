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
        SetTitleBar(AppTitleBar);
        var uiSettings = new Windows.UI.ViewManagement.UISettings();
        var accentColor = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Accent);
        AppWindow.TitleBar.ButtonForegroundColor = accentColor;

        // Set sizing and center the Window
        var manager = WindowManager.Get(this);
        manager.MinHeight = 600;
        manager.MinWidth = 800;
    }

    private void SelectorBar2_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args) {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);
        System.Type pageType;

        switch (currentSelectedIndex) {
            case 0:
                pageType = typeof(ClientsPage);
                break;
            case 1:
                pageType = typeof(SamplePage);
                break;
            case 2:
                pageType = typeof(SamplePage);
                break;
            case 3:
                pageType = typeof(SamplePage);
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
