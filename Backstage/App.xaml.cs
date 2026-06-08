using Backstage.Services;
using CDO.Core.Constants;
using Meziantou.Framework.Win32;
using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace Backstage {
    public partial class App : Application {
        private Window? _window;

        public App() {
            InitializeComponent();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args) {
            // Check for stored credentials from CDO.Win
            if (CredentialManager.ReadCredential(AppConstants.AppName) is { } creds) {
                Debug.WriteLine($"Found stored credentials for {AppConstants.AppName}, initializing services...");
                AppServices.InitializeServices(creds.UserName!, creds.Password!);

                var loaded = await AppServices.LoadDataAsync();
            }
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
