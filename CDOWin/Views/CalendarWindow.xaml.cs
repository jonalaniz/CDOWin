using CDO.Core.Models;
using CDO.UI.Shared.Factories;
using CDO.UI.Shared.Helpers;
using CDOWin.Controls;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Reminders.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace CDOWin.Views;

public sealed partial class CalendarWindow : Window {

    // =========================
    // Dependencies
    // =========================
    private readonly CalendarViewModel _viewModel;
    private readonly FrameworkElement[] _dayCells = new FrameworkElement[42];

    // =========================
    // Constructor
    // =========================
    public CalendarWindow() {
        InitializeComponent();
        SetupWindow();
        _ = SetupTitleBarAsync();

        _viewModel = AppServices.CalendarViewModel;
        _viewModel.BuildCalendarDays();
        InitializeCalendarGrid();
        UpdateCalendar();
    }

    // =========================
    // Window Setup
    // =========================
    private void SetupWindow() {
        ExtendsContentIntoTitleBar = true;
    }

    private async Task SetupTitleBarAsync() {
        var uiSettings = new Windows.UI.ViewManagement.UISettings();
        var accentColor = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Accent);
        AppWindow.TitleBar.ButtonForegroundColor = accentColor;
    }

    private void InitializeCalendarGrid() {
        for (int i = 0; i < _dayCells.Length; i++) {
            var dayView = new CalendarDayView { IsCurrentMonth = false };
            dayView.ReminderClicked += OnReminderClickedAsync;
            Grid.SetRow(dayView, i / 7);
            Grid.SetColumn(dayView, i % 7);

            CalendarGrid.Children.Add(dayView);
            _dayCells[i] = dayView;
        }
    }

    private void UpdateCalendar() {
        MonthHeader.Text = _viewModel.CurrentMonth.ToString("MMMM yyyy");

        for (int i = 0; i < _dayCells.Length; i++) {
            var day = _viewModel.Days[i];
            var cell = (CalendarDayView)_dayCells[i];
            cell.IsCurrentMonth = day.IsCurrentMonth;

            if (!day.IsCurrentMonth) {
                cell.Reminders = [];
                continue;
            }

            cell.Date = day.Date;
            cell.Reminders = day.Reminders;
        }
    }

    // =========================
    // Event Handlers
    // =========================
    private void Button_Click(object sender, RoutedEventArgs e) {
        if (sender is Button button && button.Tag is string tag) {
            if (tag == "0") {
                _viewModel.DecrementMonth();
                UpdateCalendar();
            } else {
                _viewModel.IncrementMonth();
                UpdateCalendar();
            }
        }
    }

    private async void OnReminderClickedAsync(object? sender, int id) {
        if (_viewModel.GetReminderByID(id) is not Reminder reminder)
            return;

        var updateVM = new ReminderUpdateViewModel(reminder);
        var dialog = DialogFactory.UpdateDialog(this.Content.XamlRoot, $"Edit Reminder for {updateVM.Original.ClientName}");
        dialog.Content = new UpdateReminderPage(updateVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            await _viewModel.UpdateReminderAsync(id, updateVM.Updated);
            UpdateCalendar();
        }
    }

    private void ExportButton_Click(object sender, RoutedEventArgs e) {
        _viewModel.ExportReminders();
    }

    private void RootGrid_Loaded(object sender, RoutedEventArgs e) {
        // We need to set the minimum size here because the XamlRoot is not available in the constructor.
        WindowHelper.SetWindowMinSize(this, 1200, 800);
    }
}
