using Backstage.Factories;
using Backstage.Services;
using Backstage.ViewModels;
using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Reminders;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backstage.Views;

public sealed partial class BillingPage : Page {

    // =========================
    // ViewModel
    // =========================
    public BillingViewModel ViewModel { get; } = AppServices.BillingViewModel;
    private ReminderViewModel ReminderViewModel { get; } = AppServices.ReminderViewModel;

    // =========================
    // Constructor
    // =========================
    public BillingPage() {
        InitializeComponent();
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
            ViewModel.LoadRecentSAs(force),
            ViewModel.LoadNewPlacements(force),
            ViewModel.LoadUnbilledSAs(force),
            ViewModel.LoadExpiringSAsAsync(force)
        };

        await Task.WhenAll(tasks);
    }

    // =========================
    // Click Handlers
    // =========================

    private async void Refresh_Click(object sender, RoutedEventArgs e) {
        RefreshButton.IsEnabled = false;
        await RefreshAsync(force: true);
        RefreshButton.IsEnabled = true;
    }

    private async void MarkExpiredBilled_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        var result = await ViewModel.MarkSABilled(id);
        if (result.IsSuccess) ViewModel.RemoveExpiredSA(id);
        await ShowMessage(MessageType.MarkedBilled, result.IsSuccess);
    }

    private async void MarkBilled_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        var result = await ViewModel.MarkSABilled(id);
        if (result.IsSuccess) ViewModel.RemoveUnbilledSA(id);
        await ShowMessage(MessageType.MarkedBilled, result.IsSuccess);
    }

    private async void CreateSAReminder_Today_Click(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item
            || item.Tag is not int id
            || ViewModel.ExpiredSA(id) is not AdminSASummary sa) return;
        var reminder = ReminderFactory.CreateSAReminder(sa.ClientID, ReminderDate.Today, sa.ServiceAuthorizationNumber, SAReminderType.StaleSA);
        await CreateReminder(reminder);
    }

    private async void CreateSAReminder_Tomorrow_Click(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item
            || item.Tag is not int id
            || ViewModel.ExpiredSA(id) is not AdminSASummary sa) return;
        var reminder = ReminderFactory.CreateSAReminder(sa.ClientID, ReminderDate.Tomorrow, sa.ServiceAuthorizationNumber, SAReminderType.StaleSA);
        await CreateReminder(reminder);
    }

    private async Task CreateReminder(NewReminder reminder) {
        var result = await ReminderViewModel.CreateReminderAsync(reminder);
        await ShowMessage(MessageType.CreatedReminder, result.IsSuccess);
    }

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
