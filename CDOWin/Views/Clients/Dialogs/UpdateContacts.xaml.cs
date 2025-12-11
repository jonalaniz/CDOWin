using CDOWin.Controls;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateContacts : Page {
    public ClientUpdateViewModel ViewModel { get; private set; }

    public UpdateContacts(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.OriginalClient;
        InitializeComponent();
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        string? originalValue = null;
        string? updatedValue = null;
        ContactField? field = null;

        if (sender is LabeledTextBox pair) {
            originalValue = pair.Value;
            updatedValue = pair.innerTextBox.Text;
            if (pair.TextBoxTag is ContactField f)
                field = f;
        }

        if (field == null || originalValue == updatedValue || updatedValue == null)
            return;

        UpdateValue(updatedValue, field.Value);
    }

    private void UpdateValue(string value, ContactField type) {
        switch (type) {
            case ContactField.Phone1:
                ViewModel.UpdatedClient.phone1 = value;
                break;
            case ContactField.Phone1Identity:
                ViewModel.UpdatedClient.phone1Identity = value;
                break;
            case ContactField.Phone2:
                ViewModel.UpdatedClient.phone2 = value;
                break;
            case ContactField.Phone2Identity:
                ViewModel.UpdatedClient.phone2Identity = value;
                break;
            case ContactField.Phone3:
                ViewModel.UpdatedClient.phone3 = value;
                break;
            case ContactField.Phone3Identity:
                ViewModel.UpdatedClient.phone3Identity = value;
                break;
            case ContactField.Email:
                ViewModel.UpdatedClient.email = value;
                break;
            case ContactField.EmailIdentity:
                ViewModel.UpdatedClient.emailIdentity = value;
                break;
            case ContactField.Email2:
                ViewModel.UpdatedClient.email2 = value;
                break;
            case ContactField.Email2Identity:
                ViewModel.UpdatedClient.email2Identity = value;
                break;
        }
    }
}
