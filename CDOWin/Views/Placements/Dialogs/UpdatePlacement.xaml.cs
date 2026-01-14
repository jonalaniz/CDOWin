using CDO.Core.Models;
using CDOWin.Extensions;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CDOWin.Views.Placements.Dialogs;

public sealed partial class UpdatePlacement : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly List<Employer> _employers = AppServices.EmployersViewModel.GetEmployers();
    private readonly PlacementUpdateViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public UpdatePlacement(PlacementUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        SetupNumberBoxes();
        SetupAutoSuggestionBox();
        SetupDatePickers();
    }

    // =========================
    // Constructor
    // =========================
    private void SetupNumberBoxes() {
        if(ViewModel.Original.PlacementNumber is int number)
            PlacementNumber.Value = (double)number;

        if (ViewModel.Original.DaysOnJob is float days)
            DaysOnJob.Value = (double)days;
    }

    private void SetupAutoSuggestionBox() {
        if (ViewModel.Original.CounselorName is string name)
            EmployerAutoSuggest.Text = name;
    }

    private void SetupDatePickers() {
        if (ViewModel.Original.HireDate is DateTime startDate)
            HireDatePicker.Date = startDate;

        if(ViewModel.Original.EndDate is DateTime endDate)
            EndDatePicker.Date = endDate;

        if (TryParseDateString(ViewModel.Original.FirstFiveDays1, out var day1))
            Day1Picker.Date = day1;

        if (TryParseDateString(ViewModel.Original.FirstFiveDays2, out var day2))
            Day2Picker.Date = day2;

        if (TryParseDateString(ViewModel.Original.FirstFiveDays3, out var day3))
            Day3Picker.Date = day3;

        if (TryParseDateString(ViewModel.Original.FirstFiveDays4, out var day4))
            Day4Picker.Date = day4;

        if (TryParseDateString(ViewModel.Original.FirstFiveDays5, out var day5))
            Day5Picker.Date = day5;
    }

    // =========================
    // Even Handlers
    // =========================
    private void DropDownSelected(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item || item.Tag is not ServiceAuthorization sa)
            return;
        SANumberDropDownButton.Content = sa.Id;
        ViewModel.Updated.PoNumber = sa.Id;
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {
        if (sender.Tag is not UpdateField field || double.IsNaN(sender.Value)) return;

        switch (field) {
            case UpdateField.PlacementNumber:
                ViewModel.Updated.PlacementNumber = (int)sender.Value;
                break;
            case UpdateField.FormattedSalary:
                ViewModel.Updated.Salary = sender.Value.ToString("C");
                break;
            case UpdateField.DaysOnJob:
                ViewModel.Updated.DaysOnJob = (float)sender.Value;
                break;

        }
    }

    private void TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textBox || textBox.Tag is not UpdateField field)
            return;

        var text = textBox.Text.NormalizeString();
        if (string.IsNullOrWhiteSpace(text)) return;
        UpdateValue(text, field);
    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender.Tag is UpdateField field && sender.Date is DateTimeOffset offset) {
            var dateString = offset.DateTime.Date.ToString(format: "MM/dd/yyyy");
            switch (field) {
                case UpdateField.FormattedHireDate:
                    ViewModel.Updated.HireDate = offset.DateTime.Date.ToUniversalTime();
                    break;
                case UpdateField.FormattedEndDate:
                    ViewModel.Updated.EndDate = offset.DateTime.Date.ToUniversalTime();
                    break;
                case UpdateField.Day1:
                    ViewModel.Updated.FirstFiveDays1 = dateString;
                    break;
                case UpdateField.Day2:
                    ViewModel.Updated.FirstFiveDays2 = dateString;
                    break;
                case UpdateField.Day3:
                    ViewModel.Updated.FirstFiveDays3 = dateString;
                    break;
                case UpdateField.Day4:
                    ViewModel.Updated.FirstFiveDays4 = dateString;
                    break;
                case UpdateField.Day5:
                    ViewModel.Updated.FirstFiveDays5 = dateString;
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
                .Where(c => !string.IsNullOrWhiteSpace(c.Name) && c.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            sender.ItemsSource = suggestions;
        }
    }

    private void EmployerAutoSuggest_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
        if (args.SelectedItem is Employer selectedEmployer) {
            var result = _employers.FirstOrDefault(e => e.Id == selectedEmployer.Id);
            if (result != null) {
                UpdateSelectedEmployer(result);
                sender.Text = result.Name;
            }
        }
    }

    private void EmployerAutoSuggest_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
        if (args.ChosenSuggestion is Employer chosenEmployer) {
            var result = _employers.FirstOrDefault(e => e.Id == chosenEmployer.Id);
            if (result != null) { UpdateSelectedEmployer(result); }
        } else if (!string.IsNullOrWhiteSpace(args.QueryText)) {
            // Optional: match typed text even if not chosen from suggestions
            var result = _employers.FirstOrDefault(c => c.Name.Equals(args.QueryText, StringComparison.OrdinalIgnoreCase));
            if (result != null) { UpdateSelectedEmployer(result); }
        }
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateValue(string value, UpdateField field) {
        var text = value.NormalizeString();
        if (string.IsNullOrWhiteSpace(text)) return;

        switch (field) {
            case UpdateField.Supervisor:
                ViewModel.Updated.Supervisor = text;
                break;
            case UpdateField.SupervisorPhone:
                ViewModel.Updated.SupervisorPhone = text;
                break;
            case UpdateField.SupervisorEmail:
                ViewModel.Updated.SupervisorEmail = text;
                break;
            case UpdateField.Position:
                ViewModel.Updated.Position = text;
                break;
            case UpdateField.HoursWorked:
                ViewModel.Updated.NumbersOfHoursWorking = text;
                break;
            case UpdateField.HourlyWage:
                ViewModel.Updated.HourlyOrMonthlyWages = text;
                break;
            case UpdateField.Website:
                ViewModel.Updated.Website = text;
                break;
            case UpdateField.JobDuties:
                ViewModel.Updated.DescriptionOfDuties = text;
                break;
            case UpdateField.WorkSchedule:
                ViewModel.Updated.DescriptionOfWorkSchedule = text;
                break;
        }
    }

    private void UpdateSelectedEmployer(Employer employer) {
        ViewModel.Updated.EmployerID = employer.Id.ToString();
        ViewModel.Updated.Supervisor = employer.Supervisor;
        ViewModel.Updated.SupervisorPhone = employer.SupervisorPhone;
        ViewModel.Updated.SupervisorEmail = employer.SupervisorEmail;

        SupervisorTextBox.Text = employer.Supervisor;
        SPhoneBox.Text = employer.SupervisorPhone;
        SEmailBox.Text = employer.SupervisorEmail;
    }

    // For some reason the first 5 days are stored as strings in the database.
    // Covering all sane date formats, though some will still fail as there was
    // originally no data validation/format enforcement.
    private static bool TryParseDateString(string? dateString, out DateTime date) {
        string[] formats = [
            "M/d/yy",
            "MM/d/yy",
            "M/d/yyyy",
            "MM/d/yyyy",
            "M/dd/yy",
            "MM/dd/yy", 
            "M/dd/yyyy", 
            "MM/dd/yyyy"
            ];
        return DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }
}
