using CDOWin.Controls;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Reminders.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace CDOWin.Views;

public sealed partial class CalendarWindow : Window {
    private CalendarViewModel ViewModel { get; }
    public CalendarWindow() {
        InitializeComponent();
        ViewModel = AppServices.CalendarViewModel;
        ViewModel.BuildCalendarDays();
        SetupWindow();
        BuildCalendar();
    }

    private void SetupWindow() {
        ExtendsContentIntoTitleBar = true;
        // Setup Title bar
        var uiSettings = new Windows.UI.ViewManagement.UISettings();
        var accentColor = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Accent);
        AppWindow.TitleBar.ButtonForegroundColor = accentColor;

        // Set sizing and center the Window
        var manager = WinUIEx.WindowManager.Get(this);
        manager.MinHeight = 600;
        manager.MinWidth = 800;
    }

    void BuildCalendar() {
        MonthHeader.Text = ViewModel.CurrentMonth.ToString("MMMM yyyy");
        CalendarGrid.Children.Clear();

        for (int i = 0; i < ViewModel.Days.Count; i++) {
            if (!ViewModel.Days[i].IsCurrentMonth) {
                var emptyDayView = new EmptyCalendarDay();
                Grid.SetRow(emptyDayView, i / 7);
                Grid.SetColumn(emptyDayView, i % 7);

                CalendarGrid.Children.Add(emptyDayView);
                continue;
            }

            var day = ViewModel.Days[i];
            var dayView = new CalendarDayView {
                Date = day.Date,
                Reminders = day.Reminders
            };

            dayView.ReminderClicked += OnReminderClickedAsync;

            Grid.SetRow(dayView, i / 7);
            Grid.SetColumn(dayView, i % 7);

            CalendarGrid.Children.Add(dayView);
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
        if(sender is Button button && button.Tag is string tag) {
            if (tag == "0") {
                ViewModel.DecrementMonth();
                BuildCalendar();
            } else {
                ViewModel.IncrementMonth();
                BuildCalendar();
            }
        }
    }

    private async void OnReminderClickedAsync(object? sender, int id) {
        var updateVM = new ReminderUpdateViewModel(ViewModel.GetReminderByID(id));
        var dialog = DialogFactory.UpdateDialog(this.Content.XamlRoot, $"Edit Reminder for {updateVM.Original.clientName}");
        dialog.Content = new UpdateReminderPage(updateVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            await ViewModel.UpdateReminderAsync(id, updateVM.Updated);
            BuildCalendar();
        }
    }
}
