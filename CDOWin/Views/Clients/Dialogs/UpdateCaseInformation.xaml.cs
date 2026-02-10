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

public sealed partial class UpdateCaseInformation : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly List<CounselorSummary> _counselors = AppServices.CounselorsViewModel.GetCounselors();
    public ClientUpdateViewModel ViewModel { get; private set; }

    // =========================
    // Constructor
    // =========================
    public UpdateCaseInformation(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        BuildDropDowns();
        SetupAutoSuggestBox();
        SetupDatePicker();
    }

    // =========================
    // UI Setup
    // =========================
    private void BuildDropDowns() {
        BenefitDropDown.Flyout = BuildFlyout(Benefit.All);
        StatusDropDown.Flyout = BuildFlyout(Status.All);

        var benefits = ViewModel.OriginalClient.Benefits;
        var status = ViewModel.OriginalClient.Status;

        StatusDropDown.Content = string.IsNullOrWhiteSpace(status) ? "None Set" : status;
        BenefitDropDown.Content = string.IsNullOrWhiteSpace(benefits) ? "None Set" : benefits;
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
        if (ViewModel.OriginalClient.CounselorReference is null) return;
        CounselorAutoSuggest.PlaceholderText = ViewModel.OriginalClient.CounselorReference.ToString() ?? "Type to search counselors";
    }

    private void SetupDatePicker() {
        if (ViewModel.OriginalClient.StartDate is DateTime startDate)
            StartDatePicker.Date = startDate;
    }

    // =========================
    // Property Change Methods
    // =========================
    private void StartDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (ViewModel.OriginalClient.StartDate is DateTime startDate) {
            if (startDate == StartDatePicker.Date)
                return;

            if (sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset) {
                // We call DateTime.Date to get the Date with the time zeroed out then
                // .ToUniversalTime to ensure it si in the correct format for the API
                Debug.WriteLine(offset.Date.ToUniversalTime());
                ViewModel.UpdatedClient.StartDate = offset.DateTime.Date.ToUniversalTime();
            }
        }
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
                sender.Text = result.ToString(); // display chosen name
            }
        }
    }

    private void CounselorAutoSuggest_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
        if (args.ChosenSuggestion is CounselorSummary chosenCounselor) {
            var counselor = _counselors.FirstOrDefault(c => c.Id == chosenCounselor.Id);
            if (counselor != null) { UpdateSelectedCounselor(counselor); }
        } else if (!string.IsNullOrWhiteSpace(args.QueryText)) {
            // Optional: match typed text even if not chosen from suggestions
            var counselor = _counselors.FirstOrDefault(c => c.Name.Equals(args.QueryText, StringComparison.OrdinalIgnoreCase));
            if (counselor != null) { UpdateSelectedCounselor(counselor); }
        }
    }

    private void DropDownSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            if (item.Tag is Benefit benefit) {
                ViewModel.UpdatedClient.Benefits = benefit.Value;
                BenefitDropDown.Content = benefit.Value;
            } else if (item.Tag is Status status) {
                ViewModel.UpdatedClient.Status = status.Value;
                StatusDropDown.Content = status.Value;
            }
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not CaseField field)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        UpdateValue(text, field);
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateValue(string value, CaseField type) {
        switch (type) {
            case CaseField.CaseID:
                ViewModel.UpdatedClient.CaseID = value;
                break;
            case CaseField.Premiums:
                ViewModel.UpdatedClient.Premiums = value;
                break;
        }
    }

    private void UpdateSelectedCounselor(CounselorSummary counselor) {
        // Update Labels
        CPhone.Value = counselor.Phone ?? "";
        CEmail.Value = counselor.Email ?? "";

        // Update ClientDetail
        ViewModel.UpdatedClient.CounselorID = counselor.Id;
    }
}
