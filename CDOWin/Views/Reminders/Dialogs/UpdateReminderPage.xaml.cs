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
    private readonly ReminderUpdateViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateReminderPage(ReminderUpdateViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
        SetupDatePicker();
    }

    // =========================
    // UI Setup
    // =========================
    private void SetupDatePicker() {
        if (_viewModel.Original.ActionDate is DateTime date) {
            DatePicker.Date = date;
        }
    }

    // =========================
    // Property Change Methods
    // =========================
    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (_viewModel.Original.ActionDate is DateTime date) {
            if (date == DatePicker.Date)
                return;

            if (sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset) {
                _viewModel.Updated.ActionDate = offset.DateTime.Date.ToUniversalTime();
            }
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        _viewModel.Updated.Text = text;
    }

    private void Checkbox_Clicked(object sender, RoutedEventArgs e) {
        if (sender is CheckBox checkbox) {
            _viewModel.Updated.Completed = checkbox.IsChecked;
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
        _viewModel.Updated.ActionDate = newDate.DateTime.Date.ToUniversalTime();
    }
}
