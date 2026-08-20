using CDO.Core.Models;
using CDOWin.Extensions;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace CDOWin.Views.Employers.Dialogs;

public sealed partial class UpdateEmployer : Page {

    // =========================
    // Dependencies
    // =========================
    private List<State> _states = AppServices.States();
    private readonly EmployerUpdateViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateEmployer(EmployerUpdateViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
        BuildStateDropdown();
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

        StateDropDownButton.Flyout = flyout;
    }

    private void StateSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            var state = item.Tag.ToString();
            _viewModel.Updated.State = state;
            StateDropDownButton.Content = state;
        }
    }

    // =========================
    // Property Change Methods
    // =========================
    private void LabeledTextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not Field field)
            return;

        var text = textbox.Text.NormalizeString();

        // Check if not null, all fields are optional so whitespace is allowed
        if (text == null) return;
        UpdateModel(text, field);
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateModel(string value, Field field) {
        switch (field) {
            case Field.Name:
                if (value != _viewModel.Original.Name)
                    _viewModel.Updated.Name = value;
                break;
            case Field.Address1:
                if (value != _viewModel.Original.Address1)
                    _viewModel.Updated.Address1 = value;
                break;
            case Field.Address2:
                if (value != _viewModel.Original.Address2)
                    _viewModel.Updated.Address2 = value;
                break;
            case Field.City:
                if (value != _viewModel.Original.City)
                    _viewModel.Updated.City = value;
                break;
            case Field.State:
                if (value != _viewModel.Original.State)
                    _viewModel.Updated.State = value;
                break;
            case Field.Zip:
                if (value != _viewModel.Original.Zip)
                    _viewModel.Updated.Zip = value;
                break;
            case Field.Phone:
                if (value != _viewModel.Original.Phone)
                    _viewModel.Updated.Phone = value;
                break;
            case Field.Fax:
                if (value != _viewModel.Original.Fax)
                    _viewModel.Updated.Fax = value;
                break;
            case Field.Email:
                if (value != _viewModel.Original.Email)
                    _viewModel.Updated.Email = value;
                break;
            case Field.SupervisorName:
                if (value != _viewModel.Original.SupervisorName)
                    _viewModel.Updated.SupervisorName = value;
                break;
            case Field.SupervisorPhone:
                if (value != _viewModel.Original.SupervisorPhone)
                    _viewModel.Updated.SupervisorPhone = value;
                break;
            case Field.SupervisorEmail:
                if (value != _viewModel.Original.SupervisorEmail)
                    _viewModel.Updated.SupervisorEmail = value;
                break;
            case Field.Notes:
                if (value != _viewModel.Original.Notes)
                    _viewModel.Updated.Notes = value;
                break;
            case Field.Website:
                if (value != _viewModel.Original.Website)
                    _viewModel.Updated.Website = value;
                break;
        }
    }
}
