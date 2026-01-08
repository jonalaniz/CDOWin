using CDOWin.Extensions;
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

    // =========================
    // UI Setup
    // =========================
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


    // =========================
    // Property Change Methods
    // =========================
    private void TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textBox || textBox.Tag is not Field field)
            return;

        var text = textBox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text)) return;

        switch (field) {
            case Field.Description:
                ViewModel.Updated.Description = text;
                break;
            case Field.Office:
                ViewModel.Updated.Office = text;
                break;
            case Field.UoM:
                ViewModel.Updated.UnitOfMeasurement = text;
                break;
        }
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {
        if (sender is not NumberBox numberBox || numberBox.Value <= 0.00) return;

        ViewModel.Updated.UnitCost = numberBox.Value;
    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender is not CalendarDatePicker datePicker || datePicker.Tag is not DateType dateType)
            return;

        if (datePicker.Date is DateTimeOffset offset) {
            switch (dateType) {
                case DateType.StartDate:
                    ViewModel.Updated.StartDate = offset.Date.ToUniversalTime();
                    break;
                case DateType.EndDate:
                    ViewModel.Updated.EndDate = offset.Date.ToUniversalTime();
                    break;
            }
        }
    }
}
