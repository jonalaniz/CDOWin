using CDOWin.Navigation;
using CDOWin.Services;
using CDOWin.Views.Reminders;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace CDOWin.Views;

public sealed partial class MainWindow : Window {
    private int _previousSelectedIndex = 0;
    private readonly INavigationService _navigationService;

    // =========================
    // Constructor
    // =========================
    public MainWindow() {
        InitializeComponent();
        _navigationService = AppServices.Navigation;
        _navigationService.SetFrame(ContentFrame);
        _navigationService.NavigationRequested += OnNavigationRequested;
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

    // =========================
    // Navigation
    // =========================

    private void OnNavigationRequested(CDOFrame frame) {
        var item = NavigationBar.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(mi => mi.Tag is CDOFrame tag && tag == frame);

        if (item != null)
            NavigationBar.SelectedItem = item;
    }

    private void NavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        if (args.SelectedItem is NavigationViewItem selectedItem && selectedItem.Tag is CDOFrame frame) {
            var currentSelectedIndex = sender.MenuItems.IndexOf(selectedItem);
            var direction = currentSelectedIndex - _previousSelectedIndex > 0
                ? Direction.Forward
                : Direction.Backward;
            _previousSelectedIndex = currentSelectedIndex;

            switch (frame) {
                case CDOFrame.Clients:
                    _navigationService.ShowClients(direction);
                    break;
                case CDOFrame.Counselors:
                    _navigationService.ShowCounselors(direction);
                    break;
                case CDOFrame.Employers:
                    _navigationService.ShowEmployers(direction);
                    break;
                case CDOFrame.ServiceAuthorizations:
                    _navigationService.ShowServiceAuthorizations(direction);
                    break;
                case CDOFrame.Placements:
                    _navigationService.ShowPlacements(direction);
                    break;
            }
            ;
        }
    }
}
