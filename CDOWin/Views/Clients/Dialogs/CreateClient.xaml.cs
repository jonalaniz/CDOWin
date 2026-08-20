using CDO.Core.DTOs.Counselors;
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



namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class CreateClient : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreateClientViewModel _viewModel;
    private List<CounselorSummary> _counselors = [];
    private readonly List<State> _states = AppServices.StatesViewModel.GetStates();


    // =========================
    // Initializer
    // =========================

    public CreateClient(CreateClientViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
        GetCounselorSummaries();
        BuildDropDowns();
        SetupAutoSuggestBox();
    }

    // =========================
    // UI Setup
    // =========================
    private async void GetCounselorSummaries() {
        _counselors = await AppServices.CounselorsViewModel.GetCounselors();
    }
    private void BuildDropDowns() {
        BuildStateDrowdown();
        BenefitDropDown.Flyout = BuildFlyout(Benefit.All);
        StatusDropDown.Flyout = BuildFlyout(Status.All);
        BenefitDropDown.Content = "None";
        StatusDropDown.Content = "None";
    }

    private void BuildStateDrowdown() {
        var flyout = new MenuFlyout();

        foreach (var state in _states) {
            var item = new MenuFlyoutItem {
                Text = state.ShortName,
                Tag = state.ShortName
            };

            item.Click += StateSelected;
            flyout.Items.Add(item);
        }

        StateDropDownButton.Flyout = flyout;
        StateDropDownButton.Content = "TX";
    }

    private MenuFlyout BuildFlyout(IEnumerable<dynamic> items) {
        var flyout = new MenuFlyout();
        foreach (var obj in items) {
            var item = new MenuFlyoutItem {
                Text = obj.Value,
                Tag = obj
            };

            item.Click += DropDownSelected;
            flyout.Items.Add(item);
        }

        return flyout;
    }

    private void SetupAutoSuggestBox() {
        CounselorAutoSuggest.PlaceholderText = "Type to search counselors";
    }

    // =========================
    // Event Handlers
    // =========================
    private void Expander_Expanding(Expander sender, ExpanderExpandingEventArgs args) {
        if (sender is Expander expanded && expanded.Parent is StackPanel panel) {
            foreach (var child in panel.Children) {
                if (child is Expander expander && expander != expanded)
                    expander.IsExpanded = false;
            }
        }
    }

    // =========================
    // Administrative Field Updates
    // =========================
    private void AdminTextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not AdministrativeField field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;
        UpdateValue(text, field);
    }

    // =========================
    // Personal Field Updates
    // =========================
    private void PNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {
        if (sender.Tag is not PersonalField field) return;
        if (ParseDouble(sender.Value) is not string stringValue)
            return;

        switch (field) {
            case PersonalField.DL:
                _viewModel.DriversLicense = stringValue;
                break;
            case PersonalField.SSN:
                _viewModel.Ssn = ParseSSN(stringValue);
                break;
            case PersonalField.Zip:
                _viewModel.Zip = stringValue;
                break;
        }
    }

    private void DOBPicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset)
            _viewModel.Dob = offset.DateTime.Date.ToUniversalTime();
    }

    private void PTextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not PersonalField field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;
        Debug.WriteLine(text);
        UpdateValue(text, field);
    }

    private void StateSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            var state = item.Tag.ToString();
            _viewModel.State = state;
            StateDropDownButton.Content = state;
        }
    }

    // =========================
    // Contact Field Updates
    // =========================
    private void CTextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not ContactField field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;
        UpdateValue(text, field);
    }

    // =========================
    // Case Information Updates
    // =========================
    private void StartDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset)
            _viewModel.StartDate = offset.DateTime.Date.ToUniversalTime();
    }

    // =========================
    // AutoSuggest Box Updates
    // =========================
    private void CounselorAutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
            var query = sender.Text.Trim().ToLower();
            var suggestions = _counselors
                .Where(c => c.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            sender.ItemsSource = suggestions;
        }
    }

    private void CounselorAutoSuggest_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
        if (args.SelectedItem is CounselorSummary selectedCounselor) {
            var result = _counselors.FirstOrDefault(c => c.Id == selectedCounselor.Id);
            if (result != null) {
                UpdateSelectedCounselor(result);
                sender.Text = result.ToString(); // display chosen Name
            }
        }
    }

    private void CounselorAutoSuggest_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
        if (args.ChosenSuggestion is CounselorSummary chosenCounselor) {
            var result = _counselors.FirstOrDefault(c => c.Id == chosenCounselor.Id);
            if (result != null) { UpdateSelectedCounselor(result); }
        } else if (!string.IsNullOrWhiteSpace(args.QueryText)) {
            // Optional: match typed text even if not chosen from suggestions
            var result = _counselors.FirstOrDefault(c => c.Name.Equals(args.QueryText, StringComparison.OrdinalIgnoreCase));
            if (result != null) { UpdateSelectedCounselor(result); }
        }
    }

    private void DropDownSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            if (item.Tag is Benefit benefit) {
                _viewModel.Benefits = benefit.Value;
                BenefitDropDown.Content = benefit.Value;
            } else if (item.Tag is Status status) {
                _viewModel.Status = status.Value;
                StatusDropDown.Content = status.Value;
            }
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not CaseField field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;
        UpdateValue(text, field);
    }

    // =========================
    // Conditions Updates
    // =========================
    private void ATextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not ArrangementsField field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;
        UpdateValue(text, field);
    }

    // =========================
    // Employment Profile Updates
    // =========================
    private void ETextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not EmploymentField field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;
        UpdateValue(text, field);
    }

    // =========================
    // Utility Methods
    // =========================

    // Administrative Fields
    private void UpdateValue(string value, AdministrativeField type) {
        switch (type) {
            case AdministrativeField.FirstName:
                _viewModel.FirstName = value;
                break;
            case AdministrativeField.LastName:
                _viewModel.LastName = value;
                break;
        }
    }

    // Personal Fields
    private void UpdateValue(string value, PersonalField type) {
        switch (type) {
            case PersonalField.Languages:
                _viewModel.FluentLanguages = value;
                break;
            case PersonalField.Race:
                _viewModel.Race = value;
                break;
            case PersonalField.Address1:
                _viewModel.Address1 = value;
                break;
            case PersonalField.Address2:
                _viewModel.Address2 = value;
                break;
            case PersonalField.City:
                _viewModel.City = value;
                break;
            case PersonalField.Education:
                _viewModel.Education = value;
                break;
        }
    }

    // Contact Fields
    private void UpdateValue(string value, ContactField type) {
        switch (type) {
            case ContactField.Phone1:
                _viewModel.Phone1 = value;
                break;
            case ContactField.Phone1Identity:
                _viewModel.Phone1Identity = value;
                break;
            case ContactField.Phone2:
                _viewModel.Phone2 = value;
                break;
            case ContactField.Phone2Identity:
                _viewModel.Phone2Identity = value;
                break;
            case ContactField.Phone3:
                _viewModel.Phone3 = value;
                break;
            case ContactField.Phone3Identity:
                _viewModel.Phone3Identity = value;
                break;
            case ContactField.Email:
                _viewModel.Email = value;
                break;
            case ContactField.EmailIdentity:
                _viewModel.EmailIdentity = value;
                break;
            case ContactField.Email2:
                _viewModel.Email2 = value;
                break;
            case ContactField.Email2Identity:
                _viewModel.Email2Identity = value;
                break;
        }
    }

    // Case Information Fields
    private void UpdateValue(string value, CaseField type) {
        switch (type) {
            case CaseField.CaseID:
                _viewModel.CaseID = value;
                break;
            case CaseField.Premiums:
                _viewModel.Premiums = value;
                break;
        }
    }

    private void UpdateSelectedCounselor(CounselorSummary counselor) {
        Debug.WriteLine(counselor.Name);
        _viewModel.CounselorID = counselor.Id;
        _viewModel.CounselorName = counselor.Name;
    }

    // Conditions Fields
    private void UpdateValue(string value, ArrangementsField type) {
        switch (type) {
            case ArrangementsField.EmploymentGoal:
                _viewModel.EmploymentGoal = value;
                break;
            case ArrangementsField.Conditions:
                _viewModel.Conditions = value;
                break;
        }
    }

    // Employment Profile Fields
    private void UpdateValue(string value, EmploymentField type) {
        switch (type) {
            case EmploymentField.Disability:
                _viewModel.Disability = value;
                break;
            case EmploymentField.CriminalCharge:
                _viewModel.CriminalCharge = value;
                break;
            case EmploymentField.Transportation:
                _viewModel.Transportation = value;
                break;
        }
    }

    private string? ParseDouble(double Value) {
        string stringValue = ((int)Value).ToString();
        if (stringValue.Length < 11)
            return stringValue;
        return null;
    }

    private int? ParseSSN(string value) {
        var sanitizedValue = value.Trim();
        if (sanitizedValue.Length <= 11) {
            if (int.TryParse(value, out int x)) {
                return x;
            }
        }
        return null;
    }
}
