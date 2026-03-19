using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateArrangements : Page {

    // =========================
    // Dependencies
    // =========================
    public ClientUpdateViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateArrangements(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
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
                ViewModel.UpdatedClient.EmploymentGoal = value;
                break;
            case ArrangementsField.Conditions:
                ViewModel.UpdatedClient.Conditions = value;
                break;
        }
    }
}
