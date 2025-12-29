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

public sealed partial class CreateEmployer : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreateEmployerViewModel ViewModel;
    private List<State> _states;

    // =========================
    // Constructor
    // =========================
    public CreateEmployer(CreateEmployerViewModel viewModel) {
        var states = AppServices.StatesViewModel.States.ToList();
        _states = states;
        ViewModel = viewModel;
        DataContext = viewModel;
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

    // =========================
    // Property Change Methods
    // ========================== 
    private void StateSelected(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            var state = item.Tag.ToString();
            ViewModel.State = state;
            StateDropDownButton.Content = state;
        }
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        if (sender is not FrameworkElement fe || fe.Tag is not Field field)
            return;

        string? text = sender switch {
            LabeledTextBox p => p.innerTextBox.Text,
            LabeledMultiLinePair p => p.innerTextBox.Text,
            _ => null
        };

        text = text?.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        ViewModel.UpdateField(field, text);
    }
}
