using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CDOWin.Views.Placements.Dialogs;

public sealed partial class CreatePlacements : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly List<Employer> _employers = AppServices.EmployersViewModel.GetEmployers();
    private readonly CreatePlacementViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public CreatePlacements(CreatePlacementViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
    }

    // =========================
    // UI Setup
    // =========================
    private void BuildDropDowns() {
        var flyout = new MenuFlyout();
        //foreach(var employer in employers)
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {

    }

    private void TextChanged(object sender, TextChangedEventArgs e) {

    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender.Tag is UpdateField field && sender.Date is DateTimeOffset offset) {
            var dateString = offset.DateTime.Date.ToString(format: "MM/dd/yyyy");
            switch (field) {
                case UpdateField.FormattedHireDate:
                    ViewModel.HireDate = offset.DateTime.Date.ToUniversalTime();
                    break;
                case UpdateField.FormattedEndDate:
                    ViewModel.EndDate = offset.DateTime.Date.ToUniversalTime();
                    break;
                case UpdateField.Day1:
                    ViewModel.FirstFiveDays1 = dateString;
                    break;
                case UpdateField.Day2:
                    ViewModel.FirstFiveDays2 = dateString;
                    break;
                case UpdateField.Day3:
                    ViewModel.FirstFiveDays3 = dateString;
                    break;
                case UpdateField.Day4:
                    ViewModel.FirstFiveDays4 = dateString;
                    break;
                case UpdateField.Day5:
                    ViewModel.FirstFiveDays5 = dateString;
                    break;
            }
        }
    }

    // =========================
    // AutoSuggest Box Updates
    // =========================
    private void EmployerAutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
            var query = sender.Text.Trim().ToLower();
            var suggestions = _employers
                .Where(c => c.Name.ToLower().Contains(query))
                .Select(c => c.Name)
                .ToList();

            sender.ItemsSource = suggestions;
        }
    }

    private void EmployerAutoSuggest_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
        if(args.SelectedItem is string name) {

        }
    }

    private void EmployerAutoSuggest_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
    }



    // When the employer is selected, prefill the supervisor/phone/email if the item is empty
}
