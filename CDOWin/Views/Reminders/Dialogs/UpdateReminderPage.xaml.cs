using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace CDOWin.Views.Reminders.Dialogs;

public sealed partial class UpdateReminderPage : Page {
    public ReminderUpdateViewModel ViewModel;

    public UpdateReminderPage(ReminderUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.Original;
        InitializeComponent();
        SetupDatePicker();
    }

    private void SetupDatePicker() {
        Debug.WriteLine($"Local Date: {ViewModel.Original.localDate}");
        Debug.WriteLine($"Full Date: {ViewModel.Original.date}");

        if (ViewModel.Original.date is DateTime date) {
            DatePicker.Date = date;
        }
    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (ViewModel.Original.date is DateTime date) {
            if (date == DatePicker.Date)
                return;

            if (sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset) {
                ViewModel.Updated.date = offset.DateTime.Date.ToUniversalTime();
            }
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        ViewModel.Updated.description = text;
    }

    private void Checkbox_Clicked(object sender, RoutedEventArgs e) {
        if (sender is CheckBox checkbox) {
            ViewModel.Updated.complete = checkbox.IsChecked;
        }
    }
}
