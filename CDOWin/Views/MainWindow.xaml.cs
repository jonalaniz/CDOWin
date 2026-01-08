using CDOWin.Navigation;
using CDOWin.Services;
using CDOWin.Views.Reminders;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace CDOWin.Views;

public sealed partial class MainWindow : Window {
    private readonly INavigationService _navigationService;

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

        var manager = WinUIEx.WindowManager.Get(this);
        manager.MinHeight = 800;
        manager.MinWidth = 1200;
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
}
