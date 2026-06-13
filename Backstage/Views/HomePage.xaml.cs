using Backstage.Services;
using Backstage.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading.Tasks;

namespace Backstage.Views;

public sealed partial class HomePage : Page {

    // =========================
    // ViewModel
    // =========================
    public HomeViewModel ViewModel { get; } = AppServices.HomeViewModel;

    // =========================
    // Constructor
    // =========================
    public HomePage() {
        InitializeComponent();
        SetupHelloText();
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await RefreshAsync();
    }

    private async Task RefreshAsync(bool force = false) {
        await ViewModel.LoadRecentClientsAsync(force);
        await ViewModel.LoadRecentNotesAsync(force);
        await ViewModel.LoadRecentRemindersAsync(force);
        await ViewModel.LoadExpiringSAsAsync(force);
        await ViewModel.LoadStaleClientsAsync(force);
    }


    private void SetupHelloText() {
        HelloText.Text = $"Hello, {UserName()}";
    }

    private string UserName() {
        return char.ToUpper(Environment.UserName[0]) + Environment.UserName[1..];
    }

    private async void Refresh_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        RefreshButton.IsEnabled = false;
        await RefreshAsync(force: true);
        RefreshButton.IsEnabled = true;
    }
}
