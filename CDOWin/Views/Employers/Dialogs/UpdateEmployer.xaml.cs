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
            ViewModel.Updated.State = state;
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
                if (value != ViewModel.Original.Name)
                    ViewModel.Updated.Name = value;
                break;
            case Field.Address1:
                if (value != ViewModel.Original.Address1)
                    ViewModel.Updated.Address1 = value;
                break;
            case Field.Address2:
                if (value != ViewModel.Original.Address2)
                    ViewModel.Updated.Address2 = value;
                break;
            case Field.City:
                if (value != ViewModel.Original.City)
                    ViewModel.Updated.City = value;
                break;
            case Field.State:
                if (value != ViewModel.Original.State)
                    ViewModel.Updated.State = value;
                break;
            case Field.Zip:
                if (value != ViewModel.Original.Zip)
                    ViewModel.Updated.Zip = value;
                break;
            case Field.Phone:
                if (value != ViewModel.Original.Phone)
                    ViewModel.Updated.Phone = value;
                break;
            case Field.Fax:
                if (value != ViewModel.Original.Fax)
                    ViewModel.Updated.Fax = value;
                break;
            case Field.Email:
                if (value != ViewModel.Original.Email)
                    ViewModel.Updated.Email = value;
                break;
            case Field.Supervisor:
                if (value != ViewModel.Original.Supervisor)
                    ViewModel.Updated.Supervisor = value;
                break;
            case Field.SupervisorPhone:
                if (value != ViewModel.Original.SupervisorPhone)
                    ViewModel.Updated.SupervisorPhone = value;
                break;
            case Field.SupervisorEmail:
                if (value != ViewModel.Original.SupervisorEmail)
                    ViewModel.Updated.SupervisorEmail = value;
                break;
            case Field.Notes:
                if (value != ViewModel.Original.Notes)
                    ViewModel.Updated.Notes = value;
                break;
        }
    }
}
