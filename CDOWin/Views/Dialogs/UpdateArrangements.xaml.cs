using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateArrangements : Page {
    public ClientUpdateViewModel ViewModel;

    public UpdateArrangements(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.OriginalClient;
        InitializeComponent();
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        string? originalValue = null;
        string? updatedValue = null;
        ArrangementsField? field = null;

        if (sender is LabeledMultiLinePair multiLinePair) {
            originalValue = multiLinePair.Value.NormalizeString();
            updatedValue = multiLinePair.innerTextBox.Text.NormalizeString();
            if (multiLinePair.TextBoxTag is ArrangementsField f)
                field = f;
        }

        if (field == null || originalValue == updatedValue || updatedValue == null)
            return;

        UpdateValue(updatedValue, field.Value);
    }

    private void UpdateValue(string value, ArrangementsField type) {
        switch (type) {
            case ArrangementsField.EmploymentGoal:
                ViewModel.UpdatedClient.employmentGoal = value;
                break;
            case ArrangementsField.Conditions:
                ViewModel.UpdatedClient.conditions = value;
                break;
        }
    }
}
