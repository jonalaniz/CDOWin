using CDOWin.Controls;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WinUIEx;


namespace CDOWin.Views;

public sealed partial class CalendarWindow : Window {
    private CalendarViewModel ViewModel { get; }
    public CalendarWindow() {
        ViewModel = AppServices.CalendarViewModel;
        ViewModel.BuildCalendarDays();
        InitializeComponent();
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
        CalendarGrid.Children.Clear();

        for(int i = 0; i < ViewModel.Days.Count;  i++) {
            var day = ViewModel.Days[i];
            var dayView = new CalendarDayView {
                Date = day.Date,
                Reminders = day.Reminders
            };

            Grid.SetRow(dayView, i / 7);
            Grid.SetColumn(dayView, i % 7);

            CalendarGrid.Children.Add(dayView);
        }
    }
}
