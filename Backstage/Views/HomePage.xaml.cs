using Backstage.Services;
using Backstage.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

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
        await ViewModel.LoadRecentClientsAsync();
        await ViewModel.LoadRecentNotesAsync();
        await ViewModel.LoadRecentRemindersAsync();
    }

    private void SetupHelloText() {
        HelloText.Text = $"Hello, {UserName()}";
    }

    private string UserName() {
        return char.ToUpper(Environment.UserName[0]) + Environment.UserName[1..];
    }
}
