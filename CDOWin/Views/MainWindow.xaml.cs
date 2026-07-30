using CDO.Abstractions.Navigation;
using CDO.UI.Shared.Helpers;
using CDOWin.Services;
using CDOWin.Views.Reminders;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;

namespace CDOWin.Views;

public sealed partial class MainWindow : Window {
    private readonly INavigationService<CDOFrame> _navigationService;

    // =========================
    // Constructor
    // =========================
    public MainWindow() {
        InitializeComponent();
        _navigationService = AppServices.Navigation;
        _navigationService.Initialize(NavigationBar, ContentFrame);
        SetupWindow();
        _ = SetupTitleBarAsync();

        Activated += OnActivated;
    }

    // =========================
    // Window Setup
    // =========================
    private void SetupWindow() {
        ExtendsContentIntoTitleBar = true;
        Tbar.Subtitle = GetAppVersion();
    }

    private async Task SetupTitleBarAsync() {
        await DispatcherQueue.EnqueueAsync(() => { });

        var uiSettings = new UISettings();
        var accentColor = uiSettings.GetColorValue(UIColorType.Accent);
        AppWindow.TitleBar.ButtonForegroundColor = accentColor;
    }

    private void OnActivated(object sender, WindowActivatedEventArgs args) {
        Activated -= OnActivated;
        NavigationBar.SelectedItem = NavigationBar.MenuItems.First();
        SidebarFrame.Navigate(typeof(RemindersPage), null, new SlideNavigationTransitionInfo() {
            Effect = SlideNavigationTransitionEffect.FromBottom
        });
    }

    private string GetAppVersion() {
        var version = Package.Current.Id.Version;
        return $"Version {version.Major}.{version.Minor}.{version.Build}";
    }

    private void Tbar_BackRequested(Microsoft.UI.Xaml.Controls.TitleBar sender, object args) {
        _navigationService.BackRequested();
    }

    private void RootGrid_Loaded(object sender, RoutedEventArgs e) {
        // We need to set the minimum size here because the XamlRoot is not available in the constructor.
        WindowHelper.SetWindowMinSize(this, 1200, 800);
    }
}
