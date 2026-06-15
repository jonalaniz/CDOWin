using Backstage.Factories;
using Backstage.Services;
using Backstage.ViewModels;
using CDO.Core.Constants;
using CDO.Core.DTOs.Reminders;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        var tasks = new List<Task> {
            ViewModel.LoadRecentClientsAsync(force),
            ViewModel.LoadRecentNotesAsync(force),
            ViewModel.LoadRecentRemindersAsync(force),
            ViewModel.LoadExpiringSAsAsync(force),
            ViewModel.LoadStaleClientsAsync(force),
        };

        await Task.WhenAll(tasks);
    }


    private void SetupHelloText() {
        HelloText.Text = $"Hello, {UserName()}";
    }

    private string UserName() {
        return char.ToUpper(Environment.UserName[0]) + Environment.UserName[1..];
    }


    // =========================
    // Click Handlers
    // =========================
    private async void Refresh_Click(object sender, RoutedEventArgs e) {
        RefreshButton.IsEnabled = false;
        await RefreshAsync(force: true);
        RefreshButton.IsEnabled = true;
    }

    private void MarkBilled_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        Debug.WriteLine($"Mark SA: {id} as billed.");
    }

    private void MarkInactive_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        Debug.WriteLine($"Mark {id} inactive.");
    }

    // Reminders

    private async void CreateSAReminder_Today_Click(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item
            || item.Tag is not int id
            || ViewModel.SANumberForId(id) is not string saNumber) return;
        Debug.WriteLine("fuck ass shit");
        var reminder = ReminderFactory.CreateSAReminder(id, ReminderDate.Today, saNumber, SAReminderType.StaleSA);
        await CreateReminder(reminder);
    }

    private async void CreateSAReminder_Tomorrow_Click(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item
            || item.Tag is not int id
            || ViewModel.SANumberForId(id) is not string saNumber) return;
        var reminder = ReminderFactory.CreateSAReminder(id, ReminderDate.Tomorrow, saNumber, SAReminderType.StaleSA);
        await CreateReminder(reminder);
    }

    private async void CreateClientReminder_Today_Click(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item || item.Tag is not int id) return;
        var reminder = ReminderFactory.CreateClientReminder(id, ReminderDate.Today);
        await CreateReminder(reminder);
    }

    private async void CreateClientReminder_Tomorrow_Click(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item || item.Tag is not int id) return;
        var reminder = ReminderFactory.CreateClientReminder(id, ReminderDate.Tomorrow);
        await CreateReminder(reminder);
    }

    private async Task CreateReminder(NewReminder reminder) {
        var result = await ViewModel.CreateReminderAsync(reminder);
        await ShowMessage(MessageType.CreatedReminder, result.IsSuccess);
    }

    // Utility Methods

    private async Task ShowMessage(MessageType type, bool success) {
        var infoBar = new InfoBar {
            Title = success ? "Success" : "Failed",
            Severity = success ? InfoBarSeverity.Success : InfoBarSeverity.Error,
            Message = Messages.MessageForType(type, success),
            IsOpen = true
        };

        InfoBarContainer.Children.Add(infoBar);

        await Task.Delay(3000);
        InfoBarContainer.Children.Remove(infoBar);
    }
}