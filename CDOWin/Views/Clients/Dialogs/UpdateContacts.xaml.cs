using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateContacts : Page {

    // =========================
    // Dependencies
    // =========================
    public ClientUpdateViewModel ViewModel { get; private set; }

    // =========================
    // Constructor
    // =========================
    public UpdateContacts(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
    }

    // =========================
    // Property Change Methods
    // =========================
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not ContactField field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;

        UpdateValue(text, field);
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateValue(string value, ContactField type) {
        switch (type) {
            case ContactField.Phone1:
                ViewModel.UpdatedClient.Phone1 = value;
                break;
            case ContactField.Phone1Identity:
                ViewModel.UpdatedClient.Phone1Identity = value;
                break;
            case ContactField.Phone2:
                ViewModel.UpdatedClient.Phone2 = value;
                break;
            case ContactField.Phone2Identity:
                ViewModel.UpdatedClient.Phone2Identity = value;
                break;
            case ContactField.Phone3:
                ViewModel.UpdatedClient.Phone3 = value;
                break;
            case ContactField.Phone3Identity:
                ViewModel.UpdatedClient.Phone3Identity = value;
                break;
            case ContactField.Email:
                ViewModel.UpdatedClient.Email = value;
                break;
            case ContactField.EmailIdentity:
                ViewModel.UpdatedClient.EmailIdentity = value;
                break;
            case ContactField.Email2:
                ViewModel.UpdatedClient.Email2 = value;
                break;
            case ContactField.Email2Identity:
                ViewModel.UpdatedClient.Email2Identity = value;
                break;
        }
    }
}
