using CDOWin.Views;

namespace CDOWin.Services;

public sealed class WindowManager {
    public static WindowManager Instance { get; } = new();

    private CalendarWindow? _calendarWindow;
    private LoginWindow? _loginWindow;
    private MainWindow? _mainWindow;
    private SplashWindow? _splashWindow;

    private WindowManager() { }

    public void ShowCalendar() {
        if (_calendarWindow == null) {
            _calendarWindow = new CalendarWindow();
            _calendarWindow.Closed += (_, _) => _calendarWindow = null;
            _calendarWindow.Activate();
        } else {
            _calendarWindow.Activate();
        }
    }

    public void ShowLogin() {
        if (_loginWindow == null) {
            _loginWindow = new LoginWindow();
            _loginWindow.Closed += (_, _) => _loginWindow = null;
            _loginWindow.Activate();
        } else {
            _loginWindow.Activate();
        }
    }

    public void ShowSplash() {
        if (_splashWindow == null) {
            _splashWindow = new SplashWindow();
            _splashWindow.Closed += (_, _) => _splashWindow = null;
            _splashWindow.Activate();
        } else {
            _splashWindow.Activate();
        }
    }

    public void ShowMainWindow() {
        if (_mainWindow == null) {
            _mainWindow = new MainWindow();
            _mainWindow.Closed += _closeCalendar;
            _mainWindow.Closed += (_, _) => _mainWindow = null;
            _mainWindow.Activate();
        } else {
            _mainWindow.Activate();
        }
    }

    private void _closeCalendar(object sender, Microsoft.UI.Xaml.WindowEventArgs args) {
        if (_calendarWindow != null)
            _calendarWindow.Close();
    }

    public void CloseSplash() {
        _splashWindow?.Close();
        _splashWindow = null;
    }
}
