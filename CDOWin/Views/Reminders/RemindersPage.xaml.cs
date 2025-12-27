using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Reminders.Dialogs;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace CDOWin.Views.Reminders;

public sealed partial class RemindersPage : Page {
    public RemindersViewModel ViewModel { get; }

    public RemindersPage() {
        ViewModel = AppServices.RemindersViewModel;
        DataContext = ViewModel;
        ViewModel.ClientSpecific.CollectionChanged += ClientRemindersChanged;
        InitializeComponent();
    }

    private void ClientRemindersChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        if (ViewModel.ClientSpecific != null) {
            SelectionBar.Items[2].IsEnabled = true;
        }

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

            // Does any reminder match this date?
            bool hasReminder = ViewModel.All.Any(r => r.date.Date == day);

            if (hasReminder) {
                // Mark the date (simple highlight)
                var brush = (Brush)Application.Current.Resources["AccentAAFillColorDefaultBrush"];
                args.Item.Background = brush;
                args.Item.FontWeight = FontWeights.Bold;
            } else {
                // Reset to defaults when not a reminder date
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

    private async void Reminder_Click(SplitButton sender, SplitButtonClickEventArgs args) {
        if (sender.Tag is Int32 id) {
            var updateVM = new ReminderUpdateViewModel(ViewModel.GetReminderByID(id));
            var dialog = DialogFactory.UpdateDialog(this.XamlRoot, $"Reminder for {updateVM.Original.clientName}");
            dialog.Content = new UpdateReminderPage(updateVM);

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary) {
                _ = ViewModel.UpdateReminderAsync(id, updateVM.Updated);
            }
        }
    }

    private void ToggleCompleted_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem flyoutItem && flyoutItem.Tag is int id) {
            ViewModel.ToggleCompleted(id);
        }
    }

    private void ViewClient_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem flyoutItem && flyoutItem.Tag is int clientId) {
            ViewModel.RequestClient(clientId);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem flyoutItem && flyoutItem.Tag is int id) {
            // Here we show a scaare sheet and delete
        }
    }
}
