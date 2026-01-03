using CDOWin.Views.Clients;
using CDOWin.Views.Counselors;
using CDOWin.Views.Employers;
using CDOWin.Views.Placements;
using CDOWin.Views.Reminders;
using CDOWin.Views.ServiceAuthorizations;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using WinUIEx;

namespace CDOWin.Views;

public sealed partial class MainWindow : Window {
    private int _previousSelectedIndex = 0;

    // =========================
    // Constructor
    // =========================
    public MainWindow() {
        InitializeComponent();
        SetupWindow();
        _ = SetupTitleBarAsync();

        Activated += OnActivated;
    }

    // =========================
    // Window Setup
    // =========================

    private void SetupWindow() {
        ExtendsContentIntoTitleBar = true;

        var manager = WindowManager.Get(this);
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

    // =========================
    // Navigation
    // =========================

    private void NavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        if (args.SelectedItem is NavigationViewItem selectedItem && selectedItem.Tag is CDOFrame frame) {
            Type pageType = frame switch {
                CDOFrame.Clients => typeof(ClientsPage),
                CDOFrame.Counselors => typeof(CounselorsPage),
                CDOFrame.Employers => typeof(EmployersPage),
                CDOFrame.ServiceAuthorizations => typeof(ServiceAuthorizationsPage),
                CDOFrame.Placements => typeof(PlacementsPage),
                _ => throw new ArgumentOutOfRangeException(nameof(frame), frame, null),
            };

            var currentSelectedIndex = sender.MenuItems.IndexOf(selectedItem);
            var effect = currentSelectedIndex - _previousSelectedIndex > 0
                ? SlideNavigationTransitionEffect.FromRight
                : SlideNavigationTransitionEffect.FromLeft;
            ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = effect });
            _previousSelectedIndex = currentSelectedIndex;
        }
    }
}
