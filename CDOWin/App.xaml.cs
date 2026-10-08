using CDO.Core.Constants;
using CDOWin.Services;
using Meziantou.Framework.Win32;
using Microsoft.UI.Xaml;
using System;

namespace CDOWin;

public partial class App : Application {
    private readonly WindowManager _windowManager;

    public App() {
        AppContext.SetSwitch("System.Runtime.InteropServices.EnableCOM", true);

        _windowManager = WindowManager.Instance;
        InitializeComponent();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args) {

        if (CredentialManager.ReadCredential(AppConstants.AppName) is { } creds) {
            // Initialize services
            await AppServices.InitializeServices(creds.UserName!, creds.Password!);

            // Show the loading splash screen
            _windowManager.ShowSplash();

            // Await the app to load
            var loaded = await AppServices.LoadDataAsync();

            if (loaded == true) {
                _windowManager.ShowMainWindow();
                _windowManager.CloseSplash();
            } else {
                // here we need to add an error window
            }
        } else {
            _windowManager.ShowLogin();
        }
    }
}
