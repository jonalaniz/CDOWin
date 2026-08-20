using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateEmploymentProfile : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly ClientUpdateViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateEmploymentProfile(ClientUpdateViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
    }

    // =========================
    // Property Change Methods
    // =========================
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not EmploymentField field)
            return;

        var text = textbox.Text.NormalizeString();

        // Ensure item is not null, some values ARE optional
        if (text == null) return;
        UpdateValue(text, field);
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateValue(string value, EmploymentField type) {
        switch (type) {
            case EmploymentField.Disability:
                // Non-optional field, ensure item is not empty
                if (string.IsNullOrWhiteSpace(value)) return;
                _viewModel.UpdatedClient.Disability = value;
                break;
            case EmploymentField.CriminalCharge:
                _viewModel.UpdatedClient.CriminalCharge = value;
                break;
            case EmploymentField.Transportation:
                _viewModel.UpdatedClient.Transportation = value;
                break;
        }
    }
}
