using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.Reminders.Dialogs;

public sealed partial class UpdateReminderPage : Page {

    // =========================
    // Dependencies
    // =========================
    public ReminderUpdateViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateReminderPage(ReminderUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        SetupDatePicker();
    }

    // =========================
    // UI Setup
    // =========================
    private void SetupDatePicker() {
        if (ViewModel.Original.Date is DateTime date) {
            DatePicker.Date = date;
        }
    }

    // =========================
    // Property Change Methods
    // =========================
    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (ViewModel.Original.Date is DateTime date) {
            if (date == DatePicker.Date)
                return;

            if (sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset) {
                ViewModel.Updated.Date = offset.DateTime.Date.ToUniversalTime();
            }
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        ViewModel.Updated.Description = text;
    }

    private void Checkbox_Clicked(object sender, RoutedEventArgs e) {
        if (sender is CheckBox checkbox) {
            ViewModel.Updated.Complete = checkbox.IsChecked;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not string tag) return;

        // Try to parse the tag as an integer and unwrap datepicker date
        if (!int.TryParse(tag, out int days)) return;
        if (DatePicker.Date is not DateTimeOffset offset) return;

        // Set our date
        var newDate = offset.AddDays(days);
        DatePicker.Date = newDate;
        ViewModel.Updated.Date = newDate.DateTime.Date.ToUniversalTime();
    }
}
