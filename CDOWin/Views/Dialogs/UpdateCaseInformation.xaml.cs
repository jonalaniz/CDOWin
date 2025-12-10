using CDO.Core.Models;
using CDOWin.Controls;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateCaseInformation : Page {
    private List<Counselor> _counselors;

    public ClientUpdateViewModel ViewModel { get; private set; }

    public UpdateCaseInformation(ClientUpdateViewModel viewModel) {
        var counselors = AppServices.CounselorsViewModel.Counselors.ToList();
        _counselors = counselors;
        ViewModel = viewModel;
        DataContext = viewModel.OriginalClient;
        InitializeComponent();
        BuildDropDowns();
        SetupDatePicker();
    }

    // UI Setup

    private void BuildDropDowns() {
        BuildCounselorDropDown();
        BenefitDropDown.Flyout = BuildFlyout(Benefit.All);
        StatusDropDown.Flyout = BuildFlyout(Status.All);
        if (string.IsNullOrEmpty(ViewModel.OriginalClient.benefits))
            BenefitDropDown.Content = "None";
        if (string.IsNullOrEmpty(ViewModel.OriginalClient.status))
            StatusDropDown.Content = "None";
    }

    private void BuildCounselorDropDown() {
        var flyout = new MenuFlyout();
        foreach (var counselor in _counselors) {
            var item = new MenuFlyoutItem {
                Text = counselor.name,
                Tag = counselor
            };

            item.Click += CounselorSelected;
            flyout.Items.Add(item);
        }
        CounselorDropDown.Flyout = flyout;

        if (ViewModel.OriginalClient.counselorReference?.name is string name) {
            CounselorDropDown.Content = name;
        } else {
            CounselorDropDown.Content = "Select a Counselor";
        }
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

    private void SetupDatePicker() {
        if (ViewModel.OriginalClient.startDate is DateTime startDate)
            StartDatePicker.Date = startDate;
    }

    // Data Updates

    private void StartDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (ViewModel.OriginalClient.startDate is DateTime startDate) {
            if (startDate == StartDatePicker.Date)
                return;

            if (sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset) {
                // We call DateTime.Date to get the date with the time zeroed out then
                // .ToUniversalTime to ensure it si in the correct format for the API
                ViewModel.UpdatedClient.startDate = offset.DateTime.Date.ToUniversalTime();
            }
        }
    }

    private void CounselorSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item && item.Tag is Counselor counselor) {
            ViewModel.UpdatedClient.counselor = counselor.name;
            ViewModel.UpdatedClient.counselorID = counselor.id;
            ViewModel.UpdatedClient.counselorEmail = counselor.email;
            ViewModel.UpdatedClient.counselorPhone = counselor.phone;
            ViewModel.UpdatedClient.counselorFax = counselor.fax;
            CounselorDropDown.Content = counselor.name;
        }
    }

    private void DropDownSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            if (item.Tag is Benefit benefit) {
                ViewModel.UpdatedClient.benefit = benefit.Value;
                BenefitDropDown.Content = benefit.Value;
            } else if (item.Tag is Status status) {
                ViewModel.UpdatedClient.status = status.Value;
                StatusDropDown.Content = status.Value;
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
