using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateArrangements : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly ClientUpdateViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateArrangements(ClientUpdateViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
    }

    // =========================
    // Property Change Methods
    // =========================
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not ArrangementsField field)
            return;

        var text = textbox.Text.NormalizeString();

        // Arrangements are all optional fields, allow empty variablies
        if (text == null) return;
        UpdateValue(text, field);
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateValue(string value, ArrangementsField type) {
        switch (type) {
            case ArrangementsField.EmploymentGoal:
                _viewModel.UpdatedClient.EmploymentGoal = value;
                break;
            case ArrangementsField.Conditions:
                _viewModel.UpdatedClient.Conditions = value;
                break;
        }
    }
}
