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

    // =========================
    // Constructor
    // =========================
    public CalendarWindow() {
        InitializeComponent();
        SetupWindow();
        _ = SetupTitleBarAsync();

        ViewModel = AppServices.CalendarViewModel;
        ViewModel.BuildCalendarDays();
   
        BuildCalendar();
    }

    // =========================
    // Window Setup
    // =========================
    private void SetupWindow() {
        ExtendsContentIntoTitleBar = true;

        var manager = WinUIEx.WindowManager.Get(this);
        manager.MinHeight = 600;
        manager.MinWidth = 800;
    }

    private async Task SetupTitleBarAsync() {
        var uiSettings = new Windows.UI.ViewManagement.UISettings();
        var accentColor = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Accent);
        AppWindow.TitleBar.ButtonForegroundColor = accentColor;
    }

    void BuildCalendar() {
        MonthHeader.Text = ViewModel.CurrentMonth.ToString("MMMM yyyy");

        CalendarGrid.Children.Clear();
        int totalDays = ViewModel.Days.Count;

        for (int i = 0; i < totalDays; i++) {
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

    // =========================
    // Event Handlers
    // =========================
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
