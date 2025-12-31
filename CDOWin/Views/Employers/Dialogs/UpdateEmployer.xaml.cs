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

    private void LabeledTextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not Field field)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        UpdateModel(text, field);
    }

    private void UpdateModel(string value, Field field) {
        switch (field) {
            case Field.Name:
                if (value != ViewModel.Original.name)
                    ViewModel.Updated.name = value;
                break;
            case Field.Address1:
                if (value != ViewModel.Original.address1)
                    ViewModel.Updated.address1 = value;
                break;
            case Field.Address2:
                if (value != ViewModel.Original.address2)
                    ViewModel.Updated.address2 = value;
                break;
            case Field.City:
                if (value != ViewModel.Original.city)
                    ViewModel.Updated.city = value;
                break;
            case Field.State:
                if (value != ViewModel.Original.state)
                    ViewModel.Updated.state = value;
                break;
            case Field.Zip:
                if (value != ViewModel.Original.zip)
                    ViewModel.Updated.zip = value;
                break;
            case Field.Phone:
                if (value != ViewModel.Original.phone)
                    ViewModel.Updated.phone = value;
                break;
            case Field.Fax:
                if (value != ViewModel.Original.fax)
                    ViewModel.Updated.fax = value;
                break;
            case Field.Email:
                if (value != ViewModel.Original.email)
                    ViewModel.Updated.email = value;
                break;
            case Field.Supervisor:
                if (value != ViewModel.Original.supervisor)
                    ViewModel.Updated.supervisor = value;
                break;
            case Field.SupervisorPhone:
                if (value != ViewModel.Original.supervisorPhone)
                    ViewModel.Updated.supervisorPhone = value;
                break;
            case Field.SupervisorEmail:
                if (value != ViewModel.Original.supervisorEmail)
                    ViewModel.Updated.supervisorEmail = value;
                break;
            case Field.Notes:
                if (value != ViewModel.Original.notes)
                    ViewModel.Updated.notes = value;
                break;
        }
    }
}
