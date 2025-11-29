using CDO.Core.Constants;
using Meziantou.Framework.Win32;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CDOWin;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application {
    private Window? _window;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {

        if (CredentialManager.ReadCredential(AppConstants.AppName) is { } creds) {
            // Initialize services
            AppServices.InitializeServices(creds.UserName!, creds.Password!);


            // Show the loading splash screen
            var splashWindow = new SplashWindow();
            splashWindow.Activate();

            // Await the app to load
            var loaded = await AppServices.LoadDataAsync();

            if (loaded == true) {
                _window = new MainWindow();
                _window.Activate();
                splashWindow.Close();
            } else {
                // here we need to add an error window
            }
        } else {
            _window = new LoginWindow();
            _window.Activate();
        }
    }


}
