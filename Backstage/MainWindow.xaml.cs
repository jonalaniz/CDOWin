using CommunityToolkit.WinUI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;

namespace Backstage;

public sealed partial class MainWindow : Window {

    // =========================
    // Constructor
    // =========================
    public MainWindow() {
        InitializeComponent();
        SetupWindow();
    }

    // =========================
    // Window Setup
    // =========================
    private void SetupWindow() {
        ExtendsContentIntoTitleBar = true;
        //AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

        var manager = WinUIEx.WindowManager.Get(this);
        manager.MinHeight = 800;
        manager.MinWidth = 1200;
        Tbar.Subtitle = GetAppVersion();
    }

    private string GetAppVersion() {
        var version = Package.Current.Id.Version;
        return $"Version {version.Major}.{version.Minor}.{version.Build}";
    }
}
