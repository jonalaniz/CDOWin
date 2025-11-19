using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CDOWin;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SplashWindow : WinUIEx.WindowEx {
    public SplashWindow() {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        IsMinimizable = false;
        IsMaximizable = false;

        var manager = WindowManager.Get(this);
        manager.MinWidth = 400;
        manager.MaxWidth = 400;
        manager.MinHeight = 300;
        manager.MaxHeight = 300;


        this.CenterOnScreen();
    }
}
