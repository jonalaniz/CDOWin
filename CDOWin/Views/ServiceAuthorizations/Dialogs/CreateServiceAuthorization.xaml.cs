using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Globalization.NumberFormatting;

namespace CDOWin.Views.ServiceAuthorizations.Dialogs;

public sealed partial class CreateServiceAuthorization : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreateServiceAuthorizationsViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public CreateServiceAuthorization(CreateServiceAuthorizationsViewModel viewModel, int clientID) {
        ViewModel = viewModel;
        InitializeComponent();
        SetupDatePickers();
        SetupNumberBox();
    }

    // =========================
    // UI Setup
    // =========================
    private void SetupDatePickers() {
        DateTimeOffset date = DateTime.Now;
        StartDatePicker.Date = date;
        EndDatePicker.Date = date;
    }

    private void SetupNumberBox() {
        DecimalFormatter formatter = new() {
            IntegerDigits = 1,
            FractionDigits = 2,
            SignificantDigits = 2
        };

        NumberBox.NumberFormatter = formatter;
    }

    // =========================
    // Property Change Methods
    // =========================
    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender is not CalendarDatePicker datePicker || datePicker.Tag is not DateType dateType)
            return;

        if (datePicker.Date is DateTimeOffset offset) {
            switch (dateType) {
                case DateType.StartDate:
                    ViewModel.StartDate = offset.Date.ToUniversalTime();
                    break;
                case DateType.EndDate:
                    ViewModel.EndDate = offset.Date.ToUniversalTime();
                    break;
            }
        }
    }

    private void TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textBox || textBox.Tag is not Field field)
            return;

        var text = textBox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text)) return;

        switch (field) {
            case Field.Id:
                ViewModel.Id = text;
                break;
            case Field.Description:
                ViewModel.Description = text;
                break;
            case Field.Office:
                ViewModel.Office = text;
                break;
            case Field.UoM:
                ViewModel.UnitOfMeasurement = text;
                break;
        }
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {
        if (sender is not NumberBox numberBox || numberBox.Value <= 0.00) return;

        ViewModel.UnitCost = numberBox.Value;
    }

    // when we create the SA, we use the client's id and client's associated counselor id
}
