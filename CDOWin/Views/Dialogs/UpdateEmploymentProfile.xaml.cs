using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateEmploymentProfile : Page {
    public ClientUpdateViewModel ViewModel { get; private set; }
    public UpdateEmploymentProfile(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.OriginalClient;
        InitializeComponent();
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        string? originalValue = null;
        string? updatedValue = null;
        EmploymentField? field = null;

        if (sender is LabeledTextBox pair) {
            originalValue = pair.Value;
            updatedValue = pair.innerTextBox.Text;
            if (pair.TextBoxTag is EmploymentField f)
                field = f;
        } else if (sender is LabeledMultiLinePair multiLinePair) {
            originalValue = multiLinePair.Value.NormalizeString();
            updatedValue = multiLinePair.innerTextBox.Text.NormalizeString();
            if (multiLinePair.TextBoxTag is EmploymentField f)
                field = f;
        }

        if (field == null || originalValue == updatedValue || updatedValue == null)
            return;

        UpdateValue(updatedValue, field.Value);
    }

    private void UpdateValue(string value, EmploymentField type) {
        switch (type) {
            case EmploymentField.Disability:
                ViewModel.UpdatedClient.disability = value;
                break;
            case EmploymentField.CriminalCharge:
                ViewModel.UpdatedClient.criminalCharge = value;
                break;
            case EmploymentField.Transportation:
                ViewModel.UpdatedClient.transportation = value;
                break;
        }
    }
}
