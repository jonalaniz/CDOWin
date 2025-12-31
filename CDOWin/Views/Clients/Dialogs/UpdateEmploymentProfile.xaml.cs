using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateEmploymentProfile : Page {
    public ClientUpdateViewModel ViewModel { get; private set; }
    public UpdateEmploymentProfile(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.OriginalClient;
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not EmploymentField field)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        UpdateValue(text, field);
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
