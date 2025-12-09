using CDO.Core.Models;
using CDOWin.Controls;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateCaseInformation : Page {
    private List<Counselor> _counselors;

    public ClientUpdateViewModel ViewModel { get; private set; }

    public UpdateCaseInformation(ClientUpdateViewModel viewModel) {
        var counselors = AppServices.
        ViewModel = viewModel;
        DataContext = viewModel.OriginalClient;
        InitializeComponent();
        BuildDropDowns();
        SetupDatePicker();
    }

    // UI Setup

    private void BuildDropDowns() {
        BenefitDropDownButton.Flyout = BuildFlyout(Benefit.All);
        StatusDropDownButton.Flyout = BuildFlyout(Status.All);
        if (string.IsNullOrEmpty(ViewModel.OriginalClient.benefits))
            BenefitDropDownButton.Content = "None";
        if (string.IsNullOrEmpty(ViewModel.OriginalClient.status))
            StatusDropDownButton.Content = "None";
    }

    private MenuFlyout BuildFlyout(IEnumerable<dynamic> items) {
        var flyout = new MenuFlyout();
        foreach(var obj in items) {
            var item = new MenuFlyoutItem {
                Text = obj.Value,
                Tag = obj
            };

            item.Click += DropDownSelected;
            flyout.Items.Add(item);
        }

        return flyout;
    }

    private void SetupDatePicker() {
        if(ViewModel.OriginalClient.startDate is DateTime startDate)
            StartDatePicker.Date = startDate;
    }

    // Data Updates

    private void StartDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (ViewModel.OriginalClient.startDate is DateTime startDate) {
            if (startDate == StartDatePicker.Date)
                return;

            if(sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset) {
                // We call DateTime.Date to get the date with the time zeroed out then
                // .ToUniversalTime to ensure it si in the correct format for the API
                ViewModel.UpdatedClient.startDate = offset.DateTime.Date.ToUniversalTime();
            }
        }
    }

    private void DropDownSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            if (item.Tag is Benefit benefit) {
                ViewModel.UpdatedClient.benefit = benefit.Value;
                BenefitDropDownButton.Content = benefit.Value;
            } else if (item.Tag is Status status) {
                ViewModel.UpdatedClient.status = status.Value;
                StatusDropDownButton.Content = status.Value;
            }
        }
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        string? originalValue = null;
        string? updatedValue = null;
        CaseField? field = null;

        if (sender is LabeledTextBox pair) {
            originalValue = pair.Value;
            updatedValue = pair.innerTextBox.Text;
            if (pair.TextBoxTag is CaseField f)
                field = f;
        }

        if (field == null || originalValue == updatedValue || updatedValue == null)
            return;

        UpdateValue(updatedValue, field.Value);
    }

    private void UpdateValue(string value, CaseField type) {
        switch (type) {
            case CaseField.CaseID:
                ViewModel.UpdatedClient.caseID = value;
                break;
            case CaseField.Status:
                ViewModel.UpdatedClient.status = value;
                break;
            case CaseField.Benefit:
                ViewModel.UpdatedClient.benefit = value;
                break;
            case CaseField.Premiums:
                ViewModel.UpdatedClient.premium = value;
                break;
        }
    }
}
