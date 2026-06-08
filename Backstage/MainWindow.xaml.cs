using Backstage.Services;
using Backstage.Views;
using CDO.Abstractions.Navigation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel;

namespace Backstage;

public sealed partial class MainWindow : Window {
    private readonly INavigationService<BackstageView> _navigationService;

    // =========================
    // Constructor
    // =========================
    public MainWindow() {
        InitializeComponent();
        _navigationService = AppServices.Navigation;
        _navigationService.Initialize(NavigationView, ContentFrame);
        SetupWindow();

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
        Tbar.Subtitle = GetAppVersion();
        AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
    }

    private void OnActivated(object sender, WindowActivatedEventArgs args) {
        Activated -= OnActivated;
        NavigationView.SelectedItem = NavigationView.MenuItems.First();
    }

    // =========================
    // Utility Methods
    // =========================

    private string GetAppVersion() {
        var version = Package.Current.Id.Version;
        return $"Version {version.Major}.{version.Minor}.{version.Build}";
    }

    private void PaneToggleRequested(TitleBar sender, object args) {
        NavigationView.IsPaneOpen = !NavigationView.IsPaneOpen;
    }

    private void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        Debug.WriteLine($"Selected: {sender.SelectedItem}");
    }
}
