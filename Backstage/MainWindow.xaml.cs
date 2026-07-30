using Backstage.Services;
using Backstage.Views;
using CDO.Abstractions.Navigation;
using CDO.UI.Shared.Helpers;
using Microsoft.UI.Windowing;
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
        Tbar.Subtitle = GetAppVersion();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(Tbar);
        AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
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

    private void Tbar_BackRequested(TitleBar sender, object args) {
        _navigationService.BackRequested();
    }

    private void RootGrid_Loaded(object sender, RoutedEventArgs e) {
        // We need to set the minimum size here because the XamlRoot is not available in the constructor.
        WindowHelper.SetWindowMinSize(this, 1200, 800);
    }
}
