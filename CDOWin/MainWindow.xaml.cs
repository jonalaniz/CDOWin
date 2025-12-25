using CDOWin.Views;
using CDOWin.Views.Clients;
using CDOWin.Views.Counselors;
using CDOWin.Views.Employers;
using CDOWin.Views.Placements;
using CDOWin.Views.Reminders;
using CDOWin.Views.ServiceAuthorizations;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using WinUIEx;

namespace CDOWin;

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
        if (args.SelectedItem is NavigationViewItem selectedItem && selectedItem.Tag is CDOFrame frame) {
            var currentSelectedIndex = sender.MenuItems.IndexOf(selectedItem);
            System.Type pageType;

            switch (frame) {
                case CDOFrame.Clients:
                    pageType = typeof(ClientsPage);
                    break;
                case CDOFrame.Counselors:
                    pageType = typeof(CounselorsPage);
                    break;
                case CDOFrame.Employers:
                    pageType = typeof(EmployersPage);
                    break;
                case CDOFrame.ServiceAuthorizations:
                    pageType = typeof(ServiceAuthorizationsPage);
                    break;
                case CDOFrame.Placements:
                    pageType = typeof(PlacementsPage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(frame), frame, null);
            }

            var slideNavigationTransitionEffect = currentSelectedIndex - previousSelectedIndex > 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;
            ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });
            previousSelectedIndex = currentSelectedIndex;
        }
    }
}
