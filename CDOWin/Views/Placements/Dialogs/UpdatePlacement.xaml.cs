using CDO.Core.DTOs.SAs;
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
using System.Threading.Tasks;

namespace CDOWin.Views.Placements.Dialogs;

public sealed partial class UpdatePlacement : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly PlacementUpdateViewModel _viewModel;
    private List<Employer> _employers = [];
    private readonly List<State> _states = AppServices.States();

    // =========================
    // Constructor
    // =========================
    public UpdatePlacement(PlacementUpdateViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
        BuildStateDropdown();
        SetupAutoSuggestionBox();
        SetupDatePickers();

        _ = LoadEmployersAsync();
    }

    // =========================
    // UI Setup
    // =========================
    private void BuildStateDropdown() {
        var flyout = new MenuFlyout();

        foreach (var state in _states) {
            var item = new MenuFlyoutItem {
                Text = state.ShortName,
                Tag = state.ShortName
            };

            item.Click += StateSelected;
            flyout.Items.Add(item);
        }

        StateDropDownButton.Content = _viewModel.Original.State;
        StateDropDownButton.Flyout = flyout;
    }

    private void SetupAutoSuggestionBox() {
        if (_viewModel.Original.CounselorName is string name)
            EmployerAutoSuggest.Text = name;
    }

    private void SetupDatePickers() {
        if (_viewModel.Original.HireDate is DateTime startDate)
            HireDatePicker.Date = startDate;

        if (_viewModel.Original.EndDate is DateTime endDate)
            EndDatePicker.Date = endDate;

        if (TryParseDateString(_viewModel.Original.Day1, out var day1))
            Day1Picker.Date = day1;

        if (TryParseDateString(_viewModel.Original.Day2, out var day2))
            Day2Picker.Date = day2;

        if (TryParseDateString(_viewModel.Original.Day3, out var day3))
            Day3Picker.Date = day3;

        if (TryParseDateString(_viewModel.Original.Day4, out var day4))
            Day4Picker.Date = day4;

        if (TryParseDateString(_viewModel.Original.Day5, out var day5))
            Day5Picker.Date = day5;
    }

    private async Task LoadEmployersAsync() {
        var result = await AppServices.EmployersViewModel.GetEmployers();
        if (result == null) return;
        _employers = result;
    }

    // =========================
    // Event Handlers
    // =========================
    private void DropDownSelected(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item || item.Tag is not SADetail sa)
            return;
        SANumberDropDownButton.Content = sa.ServiceAuthorizationNumber;
        _viewModel.Updated.SaNumber = sa.ServiceAuthorizationNumber;
    }

    private void StateSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            var state = item.Tag.ToString();
            _viewModel.Updated.State = state;
            StateDropDownButton.Content = state;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textBox || textBox.Tag is not UpdateField field)
            return;

        var text = textBox.Text.NormalizeString();
        // Single null check, optional values are allowed
        if (text == null) return;
        UpdateValue(text, field);
    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender.Tag is UpdateField field && sender.Date is DateTimeOffset offset) {
            var dateString = offset.DateTime.Date.ToString(format: "MM/dd/yyyy");
            switch (field) {
                case UpdateField.HireDate:
                    _viewModel.Updated.HireDate = offset.DateTime.Date.ToUniversalTime();
                    break;
                case UpdateField.EndDate:
                    _viewModel.Updated.EndDate = offset.DateTime.Date.ToUniversalTime();
                    SetDaysWorking();
                    break;
                case UpdateField.Day1:
                    _viewModel.Updated.Day1 = dateString;
                    break;
                case UpdateField.Day2:
                    _viewModel.Updated.Day2 = dateString;
                    break;
                case UpdateField.Day3:
                    _viewModel.Updated.Day3 = dateString;
                    break;
                case UpdateField.Day4:
                    _viewModel.Updated.Day4 = dateString;
                    break;
                case UpdateField.Day5:
                    _viewModel.Updated.Day5 = dateString;
                    break;
            }
        }
    }

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item || item.Tag is not UpdateField field) return;
        switch (field) {
            case UpdateField.HireDate:
                HireDatePicker.Date = null;
                break;
            case UpdateField.EndDate:
                EndDatePicker.Date = null;
                break;
        }
        SetDaysWorking();
    }

    // =========================
    // AutoSuggest Box Updates
    // =========================
    private void EmployerAutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
            _viewModel.Updated.EmployerName = sender.Text;
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

    // =========================
    // Utility Methods
    // =========================
    private void SetDaysWorking() {
        if (HireDatePicker.Date is not DateTimeOffset startDate
            || EndDatePicker.Date is not DateTimeOffset endDate) {
            _viewModel.Updated.DaysOnJob = null;
            return;
        }
        var daysOnJob = endDate - startDate;
        if (daysOnJob.Days > 0 && _viewModel.Original.DaysOnJob != daysOnJob.Days)
            _viewModel.Updated.DaysOnJob = daysOnJob.Days;
    }

    private void UpdateValue(string value, UpdateField field) {
        var text = value.NormalizeString();
        if (string.IsNullOrEmpty(text)) return;

        switch (field) {
            case UpdateField.EmployerPhone:
                _viewModel.Updated.EmployerPhone = text;
                break;
            case UpdateField.Address1:
                _viewModel.Updated.Address1 = text;
                break;
            case UpdateField.Address2:
                _viewModel.Updated.Address2 = text;
                break;
            case UpdateField.City:
                _viewModel.Updated.City = text;
                break;
            case UpdateField.Zip:
                _viewModel.Updated.Zip = text;
                break;
            case UpdateField.SupervisorName:
                _viewModel.Updated.SupervisorName = text;
                break;
            case UpdateField.SupervisorPhone:
                _viewModel.Updated.SupervisorPhone = text;
                break;
            case UpdateField.SupervisorEmail:
                _viewModel.Updated.SupervisorEmail = text;
                break;
            case UpdateField.Website:
                _viewModel.Updated.Website = text;
                break;
            case UpdateField.Position:
                _viewModel.Updated.Position = text;
                break;
            case UpdateField.HoursWorking:
                _viewModel.Updated.HoursWorking = text;
                break;
            case UpdateField.Wage:
                _viewModel.Updated.Wages = text;
                break;
            case UpdateField.Benefits:
                _viewModel.Updated.Benefits = text;
                break;
            case UpdateField.JobDuties:
                _viewModel.Updated.JobDuties = text;
                break;
            case UpdateField.WorkSchedule:
                _viewModel.Updated.WorkSchedule = text;
                break;
            case UpdateField.WorkEnvironment:
                _viewModel.Updated.WorkEnvironment = text;
                break;
            case UpdateField.Accommodations:
                _viewModel.Updated.Accommodations = text;
                break;
        }
    }

    private void BilledCheckbox_Click(object sender, RoutedEventArgs e) {
        if (sender is not CheckBox checkbox) return;
        _viewModel.Updated.Billed = checkbox.IsChecked;
    }

    private void UpdateSelectedEmployer(Employer employer) {
        _viewModel.Updated.EmployerID = employer.Id.ToString();
        _viewModel.Updated.EmployerName = employer.Name;
        _viewModel.Updated.EmployerPhone = employer.Phone;
        _viewModel.Updated.Address1 = employer.Address1;
        _viewModel.Updated.Address2 = employer.Address2;
        _viewModel.Updated.City = employer.City;
        _viewModel.Updated.State = employer.State;
        _viewModel.Updated.Zip = employer.Zip;
        _viewModel.Updated.SupervisorName = employer.SupervisorName;
        _viewModel.Updated.SupervisorPhone = employer.SupervisorPhone;
        _viewModel.Updated.SupervisorEmail = employer.SupervisorEmail;
        _viewModel.Updated.Website = employer.Website;

        EmployerPhoneTextBox.Text = employer.Phone;
        AddressTextBox.Text = employer.Address1;
        Address2TextBox.Text = employer.Address2;
        CityTextBox.Text = employer.City;
        ZipTextBox.Text = employer.Zip;
        SupervisorTextBox.Text = employer.SupervisorName;
        SPhoneBox.Text = employer.SupervisorPhone;
        SEmailBox.Text = employer.SupervisorEmail;
        WebsiteTextBox.Text = employer.Website;
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
