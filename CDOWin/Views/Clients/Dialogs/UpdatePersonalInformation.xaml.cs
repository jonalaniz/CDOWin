using CDO.Core.Models;
using CDOWin.Extensions;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdatePersonalInformation : Page {
    private List<State> _states;

    public ClientUpdateViewModel ViewModel { get; private set; }

    public UpdatePersonalInformation(ClientUpdateViewModel viewModel) {
        var states = AppServices.StatesViewModel.States.ToList();
        _states = states;
        ViewModel = viewModel;
        DataContext = viewModel.OriginalClient;
        InitializeComponent();
        BuildStateDrowdown();
        SetupDatePicker();
    }

    // UI Setup

    private void BuildStateDrowdown() {
        var flyout = new MenuFlyout();

        foreach (var state in _states) {
            var item = new MenuFlyoutItem {
                Text = state.shortName,
                Tag = state.shortName
            };

            item.Click += StateSelected;
            flyout.Items.Add(item);
        }

        StateDropDownButton.Flyout = flyout;
    }

    private void SetupDatePicker() {
        if (ViewModel.OriginalClient.dob is DateTime dob)
            DOBPicker.Date = dob;
    }

    // Data Updates

    private void DOBPicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        if (ViewModel.OriginalClient.dob is DateTime dob) {
            if (dob == DOBPicker.Date)
                return;
        }
        if (sender is CalendarDatePicker picker && picker.Date is DateTimeOffset offset) {
            // We call DateTime.Date to get the date with the time zeroed out then
            // .ToUniversalTime to ensure it is in the correct format for the API.
            ViewModel.UpdatedClient.dob = offset.DateTime.Date.ToUniversalTime();
        }
    }

    private void StateSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            var state = item.Tag.ToString();
            ViewModel.UpdatedClient.state = state;
            StateDropDownButton.Content = state;
        }
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {
        if (sender.Tag is not PersonalField field)
            return;

        if (ParseDouble(sender.Value) is not string stringValue)
            return;

        switch (field) {
            case PersonalField.DL:
                ViewModel.UpdatedClient.driversLicense = stringValue;
                break;
            case PersonalField.SSN:
                ViewModel.UpdatedClient.ssn = ParseSSN(stringValue);
                break;
            case PersonalField.Zip:
                ViewModel.UpdatedClient.zip = stringValue;
                break;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not PersonalField field)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        UpdateValue(text, field);
    }

    private void UpdateValue(string value, PersonalField type) {
        switch (type) {
            case PersonalField.Languages:
                ViewModel.UpdatedClient.fluentLanguages = value;
                break;
            case PersonalField.Race:
                ViewModel.UpdatedClient.race = value;
                break;
            case PersonalField.Address1:
                ViewModel.UpdatedClient.address1 = value;
                break;
            case PersonalField.Address2:
                ViewModel.UpdatedClient.address2 = value;
                break;
            case PersonalField.City:
                ViewModel.UpdatedClient.city = value;
                break;
            case PersonalField.Education:
                ViewModel.UpdatedClient.education = value;
                break;
        }
    }

    // Utility Methods

    private string? ParseDouble(double Value) {
        string stringValue = ((int)Value).ToString();
        if (stringValue.Length < 11)
            return stringValue;
        return null;
    }

    private int? ParseSSN(string value) {
        var sanitizedValue = value.Trim();
        if (sanitizedValue.Length <= 11) {
            int x;
            if (int.TryParse(value, out x)) {
                return x;
            }
        }
        return null;
    }
}
