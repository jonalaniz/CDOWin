using CDO.Core.Constants;
using Meziantou.Framework.Win32;
using Microsoft.UI.Xaml;

namespace CDOWin;

public partial class App : Application {
    private Window? _window;

    public App() {
        InitializeComponent();
    }

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
