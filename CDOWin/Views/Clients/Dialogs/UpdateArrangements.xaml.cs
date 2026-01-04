using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateArrangements : Page {
    public ClientUpdateViewModel ViewModel;

    public UpdateArrangements(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.OriginalClient;
        InitializeComponent();
    }

    private void LabeledTextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not ArrangementsField field)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        UpdateValue(text, field);
    }

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
