using CDO.Core.Models;
using CDOWin.Extensions;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
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
        BuildDropDown();
    }

    // =========================
    // UI Setup
    // =========================
    private void BuildDropDown() {
        var flyout = new MenuFlyout();

        if (ViewModel.Client.Pos == null) return;
        foreach (var sa in ViewModel.Client.Pos) {
            var item = new MenuFlyoutItem {
                Text = sa.Description,
                Tag = sa
            };

            item.Click += DropDownSelected;
            flyout.Items.Add(item);
        }

        SANumberDropDownButton.Flyout = flyout;
    }

    // =========================
    // Even Handlers
    // =========================
    private void DropDownSelected(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item || item.Tag is not Invoice sa)
            return;
        SANumberDropDownButton.Content = sa.ServiceAuthorizationNumber;
        ViewModel.PoNumber = sa.ServiceAuthorizationNumber;
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {
        if (sender.Tag is not UpdateField field || double.IsNaN(sender.Value)) return;

        switch (field) {
            case UpdateField.PlacementNumber:
                ViewModel.PlacementNumber = (int)sender.Value;
                break;
            case UpdateField.FormattedSalary:
                ViewModel.Salary = sender.Value.ToString("C");
                break;
            case UpdateField.DaysOnJob:
                ViewModel.DaysOnJob = (float)sender.Value;
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
                ViewModel.Supervisor = text;
                break;
            case UpdateField.SupervisorPhone:
                ViewModel.SupervisorPhone = text;
                break;
            case UpdateField.SupervisorEmail:
                ViewModel.SupervisorEmail = text;
                break;
            case UpdateField.Position:
                ViewModel.Position = text;
                break;
            case UpdateField.HoursWorked:
                ViewModel.NumbersOfHoursWorking = text;
                break;
            case UpdateField.HourlyWage:
                ViewModel.HourlyOrMonthlyWages = text;
                break;
            case UpdateField.Website:
                ViewModel.Website = text;
                break;
            case UpdateField.JobDuties:
                ViewModel.DescriptionOfDuties = text;
                break;
            case UpdateField.WorkSchedule:
                ViewModel.DescriptionOfWorkSchedule = text;
                break;
        }
    }

    private void UpdateSelectedEmployer(Employer employer) {
        ViewModel.EmployerID = employer.Id;
        ViewModel.Supervisor = employer.Supervisor;
        ViewModel.SupervisorPhone = employer.SupervisorPhone;
        ViewModel.SupervisorEmail = employer.SupervisorEmail;

        SupervisorTextBox.Text = employer.Supervisor;
        SPhoneBox.Text = employer.SupervisorPhone;
        SEmailBox.Text = employer.SupervisorEmail;
    }
}
