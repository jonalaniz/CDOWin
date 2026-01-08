using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Globalization.NumberFormatting;

namespace CDOWin.Views.ServiceAuthorizations.Dialogs;

public sealed partial class UpdateSA : Page {

    // =========================
    // Dependencies
    // =========================
    public ServiceAuthorizationUpdateViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateSA(ServiceAuthorizationUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        SetupNumberBox();
        SetupDatePickers();
    }

    private void SetupDatePickers() {
        StartDatePicker.Date = ViewModel.Original.StartDate;
        EndDatePicker.Date = ViewModel.Original.EndDate;
    }

    private void SetupNumberBox() {
        DecimalFormatter formatter = new() {
            IntegerDigits = 1,
            FractionDigits = 2,
            SignificantDigits = 2
        };

        NumberBox.NumberFormatter = formatter;
        NumberBox.Value = ViewModel.Original.UnitCost ?? 0.00;
    }

    private void TextChanged(object sender, TextChangedEventArgs e) {

    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {

    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }
}
