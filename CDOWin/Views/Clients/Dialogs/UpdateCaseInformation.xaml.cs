using CDO.Core.Models;
using CDOWin.Extensions;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateCaseInformation : Page {
    private List<Counselor> _counselors;

    public ClientUpdateViewModel ViewModel { get; private set; }

    public UpdateCaseInformation(ClientUpdateViewModel viewModel) {
        var counselors = AppServices.CounselorsViewModel.All();
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
        if (string.IsNullOrEmpty(ViewModel.OriginalClient.Benefits))
            BenefitDropDown.Content = "None";
        if (string.IsNullOrEmpty(ViewModel.OriginalClient.Status))
            StatusDropDown.Content = "None";
    }

    private void BuildCounselorDropDown() {
        var flyout = new MenuFlyout();
        foreach (var counselor in _counselors) {
            var item = new MenuFlyoutItem {
                Text = counselor.Name,
                Tag = counselor
            };

            item.Click += CounselorSelected;
            flyout.Items.Add(item);
        }
        CounselorDropDown.Flyout = flyout;

        if (ViewModel.OriginalClient.CounselorReference?.Name is string name) {
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
        if (ViewModel.OriginalClient.StartDate is DateTime startDate)
            StartDatePicker.Date = startDate;
    }

    // Data Updates

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

    private void CounselorSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item && item.Tag is Counselor counselor) {
            ViewModel.UpdatedClient.Counselor = counselor.Name;
            ViewModel.UpdatedClient.CounselorID = counselor.Id;
            ViewModel.UpdatedClient.CounselorEmail = counselor.Email;
            ViewModel.UpdatedClient.CounselorPhone = counselor.Phone;
            ViewModel.UpdatedClient.CounselorFax = counselor.Fax;
            CounselorDropDown.Content = counselor.Name;
        }
    }

    private void DropDownSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            if (item.Tag is Benefit benefit) {
                ViewModel.UpdatedClient.Benefit = benefit.Value;
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

    private void UpdateValue(string value, CaseField type) {
        switch (type) {
            case CaseField.CaseID:
                ViewModel.UpdatedClient.CaseID = value;
                break;
            case CaseField.Status:
                ViewModel.UpdatedClient.Status = value;
                break;
            case CaseField.Benefit:
                ViewModel.UpdatedClient.Benefit = value;
                break;
            case CaseField.Premiums:
                ViewModel.UpdatedClient.Premium = value;
                break;
        }
    }
}
