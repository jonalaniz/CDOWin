using Backstage.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.ApplicationModel;

namespace Backstage;

public sealed partial class MainWindow : Window {
        // =========================
        // Constructor
        // =========================
        public MainWindow() {
        InitializeComponent();
        SetupWindow();

        // Set the initial page
        contentFrame.Navigate(typeof(UsersPage));
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

    // =========================
    // Utility Methods
    // =========================

    private string GetAppVersion() {
        var version = Package.Current.Id.Version;
        return $"Version {version.Major}.{version.Minor}.{version.Build}";
    }

    private void PaneToggleRequested(Microsoft.UI.Xaml.Controls.TitleBar sender, object args) {
        MainNavigationView.IsPaneOpen = !MainNavigationView.IsPaneOpen;
    }

    private void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        Debug.WriteLine($"Selected: {sender.SelectedItem}");
    }
}
