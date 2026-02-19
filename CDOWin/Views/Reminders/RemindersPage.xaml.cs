using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Reminders.Dialogs;
using CDOWin.Views.Shared.Dialogs;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace CDOWin.Views.Reminders;

public sealed partial class RemindersPage : Page {

    // =========================
    // ViewModel
    // =========================
    public RemindersViewModel ViewModel { get; } = AppServices.RemindersViewModel;

    // =========================
    // Constructor
    // =========================
    public RemindersPage() {
        ViewModel.ClientSpecific.CollectionChanged += ClientRemindersChanged;
        InitializeComponent();
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await ViewModel.LoadRemindersAsync();
    }

    // =========================
    // Property Change Methods
    // =========================
    private void ClientRemindersChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        if (SelectionBar.Items[2].IsEnabled == false)
            SelectionBar.Items[2].IsEnabled = true;

        if (SelectionBar.Items[2].IsSelected)
            return;

        SelectionBar.SelectedItem = SelectionBar.Items[2];
    }

    private void RemindersCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args) {
        SetDate();
    }

    private void RemindersCalendar_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args) {
        // Render basic day items.
        if (args.Phase == 0) {

            // Register callback for next phase.
            args.RegisterUpdateCallback(RemindersCalendar_CalendarViewDayItemChanging);

        } else if (args.Phase == 1) {

            DateTime day = args.Item.Date.Date;

            // Does any reminder match this Date?
            bool hasReminder = ViewModel.DateHasReminders(day);

            if (hasReminder) {
                // Mark the Date (simple highlight)
                var brush = (Brush)Application.Current.Resources["AccentAAFillColorDefaultBrush"];
                args.Item.Background = brush;
                args.Item.FontWeight = FontWeights.Bold;
            } else {
                // Reset to defaults when not a reminder Date
                args.Item.Background = null;
                args.Item.FontWeight = FontWeights.Normal;
            }
        }
    }

    private void SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args) {
        SelectorBarItem selectedItem = sender.SelectedItem;
        if (selectedItem.Tag is RemindersFilter filter) {
            switch (filter) {
                case RemindersFilter.Date:
                    SetDate();
                    break;
                default:
                    ViewModel.Filter = filter;
                    break;
            }
        }
    }

    // =========================
    // Click Handlers
    // =========================
    private void Calendar_Click(object sender, RoutedEventArgs e) {
        WindowManager.Instance.ShowCalendar();
    }

    private async void Reminder_Click(SplitButton sender, SplitButtonClickEventArgs args) {
        if (sender.Tag is Int32 id && ViewModel.GetReminderByID(id) is Reminder reminder) {
            var updateVM = new ReminderUpdateViewModel(reminder);
            var dialog = DialogFactory.UpdateDialog(this.XamlRoot, $"Edit Reminder for {updateVM.Original.ClientName}");
            dialog.Content = new UpdateReminderPage(updateVM);

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary) {
                _ = ViewModel.UpdateReminderAsync(id, updateVM.Updated);
            }
        }
    }

    private void ToggleCompleted_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem flyoutItem && flyoutItem.Tag is int id)
            ViewModel.ToggleCompleted(id);
    }

    private void ViewClient_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem flyoutItem && flyoutItem.Tag is int clientId) {
            ViewModel.RequestClient(clientId);
            AppServices.Navigation.Navigate(CDOFrame.Clients);
        }
    }

    private void Defer_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item && item.Tag is int id) {
            if (!int.TryParse(item.CommandParameter?.ToString(), out var days))
                return;
            ViewModel.DeferDate(id, days);
        }
    }

    private async void Delete_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem flyoutItem && flyoutItem.Tag is int id) {
            var dialog = DialogFactory.DeleteDialog(this.XamlRoot, "Delete Reminder?");
            dialog.Content = new DeletePage();

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary) {
                await ViewModel.DeleteReminderAsync(id);
            }
        }
    }

    // =========================
    // Utility Methods
    // =========================
    private void SetDate() {
        if (RemindersCalendar.SelectedDates.Count == 0) {
            ViewModel.ApplyDateFilter(DateTime.Now);
        } else if (RemindersCalendar.SelectedDates.First() is DateTimeOffset offset) {
            ViewModel.ApplyDateFilter(offset.Date);
            if (SelectionBar.Items.Last().IsSelected)
                return;

            SelectionBar.SelectedItem = SelectionBar.Items.Last();
        }
    }
}
