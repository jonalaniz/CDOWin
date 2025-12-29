using CDO.Core.Models;
using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace CDOWin.Views.Employers.Dialogs;

public sealed partial class UpdateEmployer : Page {
    private List<State> _states;

    public EmployerUpdateViewModel ViewModel;

    // Constructor
    public UpdateEmployer(EmployerUpdateViewModel viewModel) {
        var states = AppServices.StatesViewModel.States.ToList();
        _states = states;
        ViewModel = viewModel;
        DataContext = viewModel.Original;
        InitializeComponent();
        BuildStateDropdown();
    }

    private void BuildStateDropdown() {
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

    private void StateSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            var state = item.Tag.ToString();
            ViewModel.Updated.state = state;
            StateDropDownButton.Content = state;
        }
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        string? originalValue = null;
        string? updatedValue = null;
        Field? field = null;

        if (sender is LabeledMultiLinePair multiLinePair) {
            originalValue = multiLinePair.Value.NormalizeString();
            updatedValue = multiLinePair.innerTextBox.Text.NormalizeString();
            field = (Field)multiLinePair.Tag;
        } else if (sender is LabeledTextBox multiLineTextBox) {
            originalValue = multiLineTextBox.Value.NormalizeString();
            updatedValue = multiLineTextBox.innerTextBox.Text.NormalizeString();
            field = (Field)multiLineTextBox.Tag;
        }

        if (field == null || originalValue == updatedValue || updatedValue == null)
            return;

        UpdateModel(updatedValue, field.Value);
    }

    private void UpdateModel(string value, Field field) {
        switch (field) {
            case Field.Name:
                ViewModel.Updated.name = value;
                break;
            case Field.Address1:
                ViewModel.Updated.address1 = value;
                break;
            case Field.Address2:
                ViewModel.Updated.address2 = value;
                break;
            case Field.City:
                ViewModel.Updated.city = value;
                break;
            case Field.State:
                ViewModel.Updated.state = value;
                break;
            case Field.Zip:
                ViewModel.Updated.zip = value;
                break;
            case Field.Phone:
                ViewModel.Updated.phone = value;
                break;
            case Field.Fax:
                ViewModel.Updated.fax = value;
                break;
            case Field.Email:
                ViewModel.Updated.email = value;
                break;
            case Field.Supervisor:
                ViewModel.Updated.supervisor = value;
                break;
            case Field.SupervisorPhone:
                ViewModel.Updated.supervisorPhone = value;
                break;
            case Field.SupervisorEmail:
                ViewModel.Updated.supervisorEmail = value;
                break;
            case Field.Notes:
                ViewModel.Updated.notes = value;
                break;
        }
    }
}
