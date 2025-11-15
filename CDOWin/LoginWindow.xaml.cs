using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
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
        private readonly HttpClient httpClient;

        public string ServerAddress { get; private set; }
        public string ApiKey { get; private set; }
        public bool CredentialsSaved { get; private set; }

        public LoginWindow() {
            InitializeComponent();

            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);

            // Set window size and center it
            SetupWindow();

            // Subscribe to closed event for cleanup
            this.Closed += OnWindowClosed;
        }

        private void SetupWindow() {
            // Set up our fullscreen view
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            
            // Set sizing and center the Window
            var manager = WindowManager.Get(this);
            manager.MinHeight = 360;
            manager.MinWidth = 480;
            manager.MaxHeight = 360;
            manager.MaxWidth = 480;
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
                bool success = await TestConnectionAsync(
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

        private async Task<bool> TestConnectionAsync(string serverAddress, string apiKey) {
            // Simulate API call - Replace this with your actual connection test logic
            await Task.Delay(1500); // Simulate network delay

            try {
                // Example: Test a simple GET request to the server
                var request = new HttpRequestMessage(HttpMethod.Get, serverAddress);
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            } catch {
                // For demo purposes, you might want to return true/false based on your logic
                // This is where you'd implement your actual server validation
                return false;
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

        private void OnWindowClosed(object sender, WindowEventArgs args) {
            httpClient?.Dispose();
        }
    }
}
