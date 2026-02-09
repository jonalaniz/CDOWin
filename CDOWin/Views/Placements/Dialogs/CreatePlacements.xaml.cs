using CDO.Core.Models;
using CDOWin.Extensions;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace CDOWin.Views.Placements.Dialogs;

public sealed partial class CreatePlacements : Page {

    // =========================
    // Dependencies
    // =========================
    private List<Employer> _employers = [];
    private readonly CreatePlacementViewModel ViewModel;
    private List<State> _states = AppServices.StatesViewModel.States.ToList();

    // =========================
    // Constructor
    // =========================
    public CreatePlacements(CreatePlacementViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        BuildDropDown();
        BuildStateDropdown();

        _ = LoadEmployersAsync();
    }

    // =========================
    // UI Setup
    // =========================
    private void BuildDropDown() {
        var flyout = new MenuFlyout();

        if (ViewModel.Client.Invoices == null) return;
        foreach (var sa in ViewModel.Client.Invoices) {
            var item = new MenuFlyoutItem {
                Text = sa.Description,
                Tag = sa
            };

            item.Click += DropDownSelected;
            flyout.Items.Add(item);
        }

        SANumberDropDownButton.Flyout = flyout;
    }

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

        StateDropDownButton.Content = "TX";
        ViewModel.State = "TX";
        StateDropDownButton.Flyout = flyout;
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
        if (sender is not MenuFlyoutItem item || item.Tag is not Invoice sa)
            return;
        SANumberDropDownButton.Content = sa.ServiceAuthorizationNumber;
        ViewModel.SaNumber = sa.ServiceAuthorizationNumber;
    }

    private void StateSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            var state = item.Tag.ToString();
            ViewModel.State = state;
            StateDropDownButton.Content = state;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
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
                case UpdateField.HireDate:
                    ViewModel.HireDate = offset.DateTime.Date.ToUniversalTime();
                    break;
                case UpdateField.EndDate:
                    ViewModel.EndDate = offset.DateTime.Date.ToUniversalTime();
                    SetDaysWorking();
                    break;
                case UpdateField.Day1:
                    ViewModel.Day1 = dateString;
                    break;
                case UpdateField.Day2:
                    ViewModel.Day2 = dateString;
                    break;
                case UpdateField.Day3:
                    ViewModel.Day3 = dateString;
                    break;
                case UpdateField.Day4:
                    ViewModel.Day4 = dateString;
                    break;
                case UpdateField.Day5:
                    ViewModel.Day5 = dateString;
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
            ViewModel.EmployerName = sender.Text.Trim();
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
            ViewModel.DaysOnJob = null;
            Debug.WriteLine("Set days to null");
            return;
        }
        var daysOnJob = endDate - startDate;
        if (daysOnJob.Days > 0) ViewModel.DaysOnJob = daysOnJob.Days;
    }

    private void UpdateValue(string value, UpdateField field) {
        var text = value.NormalizeString();
        if (string.IsNullOrWhiteSpace(text)) return;

        switch (field) {
            case UpdateField.EmployerPhone:
                ViewModel.EmployerPhone = text;
                break;
            case UpdateField.Address1:
                ViewModel.Address1 = text;
                break;
            case UpdateField.Address2:
                ViewModel.Address2 = text;
                break;
            case UpdateField.City:
                ViewModel.City = text;
                break;
            case UpdateField.Zip:
                ViewModel.Zip = text;
                break;
            case UpdateField.SupervisorName:
                ViewModel.SupervisorName = text;
                break;
            case UpdateField.SupervisorPhone:
                ViewModel.SupervisorPhone = text;
                break;
            case UpdateField.SupervisorEmail:
                ViewModel.SupervisorEmail = text;
                break;
            case UpdateField.Website:
                ViewModel.Website = text;
                break;
            case UpdateField.Position:
                ViewModel.Position = text;
                break;
            case UpdateField.HoursWorking:
                ViewModel.HoursWorking = text;
                break;
            case UpdateField.Wage:
                ViewModel.Wages = text;
                break;
            case UpdateField.Benefits:
                ViewModel.Benefits = text;
                break;
            case UpdateField.JobDuties:
                ViewModel.JobDuties = text;
                break;
            case UpdateField.WorkSchedule:
                ViewModel.WorkSchedule = text;
                break;
        }
    }

    private void UpdateSelectedEmployer(Employer employer) {
        ViewModel.EmployerID = employer.Id;
        ViewModel.EmployerName = employer.Name;
        ViewModel.EmployerPhone = employer.Phone;
        ViewModel.Address1 = employer.Address1;
        ViewModel.Address2 = employer.Address2;
        ViewModel.City = employer.City;
        ViewModel.State = employer.State;
        ViewModel.Zip = employer.Zip;
        ViewModel.SupervisorName = employer.Supervisor;
        ViewModel.SupervisorPhone = employer.SupervisorPhone;
        ViewModel.SupervisorEmail = employer.SupervisorEmail;
        ViewModel.Website = employer.Website;

        EmployerPhoneTextBox.Text = employer.Phone;
        AddressTextBox.Text = employer.Address1;
        Address2TextBox.Text = employer.Address2;
        CityTextBox.Text = employer.City;
        ZipTextBox.Text = employer.Zip;
        SupervisorTextBox.Text = employer.Supervisor;
        SPhoneBox.Text = employer.SupervisorPhone;
        SEmailBox.Text = employer.SupervisorEmail;
        WebsiteTextBox.Text = employer.Website;
    }
}
