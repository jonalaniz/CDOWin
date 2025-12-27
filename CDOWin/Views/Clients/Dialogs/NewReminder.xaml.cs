using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class NewReminder : Page {
    NewRemindersViewModel ViewModel;
    public NewReminder(NewRemindersViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
        SetupDatePicker();
    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender is CalendarDatePicker datePicker && datePicker.Date is DateTimeOffset dateTimeOffset) {
            var dateTime = dateTimeOffset.DateTime.Date;
            ViewModel.Date = dateTime;
        }
    }

    private void LabeledMultiLinePair_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        if (sender is LabeledMultiLinePair pair && pair.innerTextBox.Text.NormalizeString() is string text)
            ViewModel.Description = text;
    }

    private void SetupDatePicker() {
        DatePicker.Date = DateTimeOffset.Now.Date;
    }
}
