using CDO.Core.Models.Enums;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using Windows.Globalization.NumberFormatting;

namespace CDOWin.Views.ServiceAuthorizations.Dialogs;

public sealed partial class UpdateSA : Page {

    // =========================
    // Dependencies
    // =========================
    public ServiceAuthorizationUpdateViewModel ViewModel;
    private readonly SAType[] _descriptions = SAType.AllItems();
    private readonly DARSOffice[] _offices = DARSOffice.AllItems();


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
            case Field.SaNumber:
                ViewModel.Updated.ServiceAuthorizationNumber = text;
                break;
            case Field.CounselorName:
                ViewModel.Updated.CounselorName = text;
                break;
            case Field.SecretaryName:
                ViewModel.Updated.SecretaryName = text;
                break;
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

    private void DescriptionSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
            var query = sender.Text.Trim().ToLower();
            var suggestions = _descriptions
                .Where(d => d.Description.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            sender.ItemsSource = suggestions;

            // Also set the description to the text in case they do not ever select.
            ViewModel.Updated.Description = sender.Text;
        }
    }

    private void DescriptionSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
        if (args.ChosenSuggestion is SAType saType) {
            ViewModel.Updated.Description = saType.Description;
            NumberBox.Value = (double)saType.Value;
            UMBox.Text = saType.UM;
        }
    }

    private void DescriptionSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
        if (args.SelectedItem is SAType saType) {
            sender.Text = saType.Description;
            ViewModel.Updated.Description = saType.Description;
            NumberBox.Value = (double)saType.Value;
            UMBox.Text = saType.UM;
        }
    }

    private void OfficeSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
            var query = sender.Text.Trim().ToLower();
            var suggestions = _offices
                .Where(o => o.Address.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            sender.ItemsSource = suggestions;

            // Also set the description to the text in case they do not ever select.
            ViewModel.Updated.Office = sender.Text;
        }
    }

    private void OfficeSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
        if (args.ChosenSuggestion is DARSOffice office)
            ViewModel.Updated.Office = office.Address;
    }

    private void OfficeSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
        if (args.SelectedItem is DARSOffice office)
            ViewModel.Updated.Office = office.Address;
    }
}
