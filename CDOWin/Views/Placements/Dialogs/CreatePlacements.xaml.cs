using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;


namespace CDOWin.Views.Placements.Dialogs;

public sealed partial class CreatePlacements : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreatePlacementViewModel ViewModel;

    public CreatePlacements(CreatePlacementViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {

    }

    private void TextChanged(object sender, TextChangedEventArgs e) {

    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender.Tag is UpdateField field && sender.Date is DateTimeOffset offset) {
            var date = offset.DateTime.Date.ToString(format: "MM/dd/yyyy");
            switch (field) {
                case UpdateField.Day1:
                    ViewModel.FirstFiveDays1 = date;
                    break;
                case UpdateField.Day2:
                    ViewModel.FirstFiveDays2 = date;
                    break;
                case UpdateField.Day3:
                    ViewModel.FirstFiveDays3 = date;
                    break;
                case UpdateField.Day4:
                    ViewModel.FirstFiveDays4 = date;
                    break;
                case UpdateField.Day5:
                    ViewModel.FirstFiveDays5 = date;
                    break;
            }
        }
    }

    private void DayPicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }

    // When the employer is selected, prefill the supervisor/phone/email if the item is empty
}
