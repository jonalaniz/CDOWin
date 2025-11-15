using CDOWin.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

namespace CDOWin {
    public sealed partial class LoginWindow : Window {
        private bool isTestSuccessful = false;
        public string ServerAddress { get; private set; } = string.Empty;
        public string ApiKey { get; private set; } = string.Empty;
        public bool CredentialsSaved { get; private set; }

        public LoginWindow() {
            InitializeComponent();

            // Set window size and center it
            SetupWindow();
        }


        private void SetupWindow() {
            // Set up our fullscreen view
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            
            // Set sizing and center the Window
            var manager = WindowManager.Get(this);
            manager.MinHeight = 430;
            manager.MinWidth = 500;
            manager.MaxHeight = 430;
            manager.MaxWidth = 500;
            this.CenterOnScreen();
        }

        private void OnCredentialsChanged(object sender, RoutedEventArgs e) {
            // Enable test button only if both fields have content
            bool hasServerAddress = !string.IsNullOrWhiteSpace(ServerAddressTextBox.Text);
            bool hasApiKey = !string.IsNullOrWhiteSpace(ApiKeyPasswordBox.Password);

            TestButton.IsEnabled = hasServerAddress && hasApiKey;

            // Reset test status when credentials change
            if (isTestSuccessful) {
                isTestSuccessful = false;
                SaveButton.IsEnabled = false;
                StatusInfoBar.IsOpen = false;
            }
        }

        private async void TestButton_Click(object sender, RoutedEventArgs e) {
            TestButton.IsEnabled = false;
            SaveButton.IsEnabled = false;

            StatusInfoBar.Severity = InfoBarSeverity.Informational;
            StatusInfoBar.Title = "Testing connection...";
            StatusInfoBar.Message = "Please wait while we verify your credentials.";
            StatusInfoBar.IsOpen = true;

            try {
                bool success = await NetworkService.IsAPIKeyValidAsync(
                    ServerAddressTextBox.Text.Trim(),
                    ApiKeyPasswordBox.Password
                );

                if (success) {
                    isTestSuccessful = true;
                    SaveButton.IsEnabled = true;

                    StatusInfoBar.Severity = InfoBarSeverity.Success;
                    StatusInfoBar.Title = "Connection successful!";
                    StatusInfoBar.Message = "Your credentials have been verified. You can now save them.";
                } else {
                    isTestSuccessful = false;

                    StatusInfoBar.Severity = InfoBarSeverity.Error;
                    StatusInfoBar.Title = "Connection failed";
                    StatusInfoBar.Message = "Unable to connect to the server. Please check your credentials.";
                }
            } catch (Exception ex) {
                isTestSuccessful = false;

                StatusInfoBar.Severity = InfoBarSeverity.Error;
                StatusInfoBar.Title = "Connection error";
                StatusInfoBar.Message = $"Error: {ex.Message}";
            } finally {
                TestButton.IsEnabled = true;
                StatusInfoBar.IsOpen = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            if (!isTestSuccessful) {
                StatusInfoBar.Severity = InfoBarSeverity.Warning;
                StatusInfoBar.Title = "Test required";
                StatusInfoBar.Message = "Please test the connection before saving.";
                StatusInfoBar.IsOpen = true;
                return;
            }

            // Save the credentials
            ServerAddress = ServerAddressTextBox.Text.Trim();
            ApiKey = ApiKeyPasswordBox.Password;
            CredentialsSaved = true;

            // TODO: Implement your credential storage logic here
            // For example: Save to secure storage, configuration file, etc.

            // Close the window
            this.Close();
        }
    }
}
