using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Counselors.Dialogs;

public sealed partial class UpdateCounselor : Page {

    // =========================
    // Dependencies
    // =========================
    public CounselorUpdateViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateCounselor(CounselorUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
    }

    // =========================
    // Property Change Methods
    // =========================
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not Field field)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        UpdateModel(text, field);
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateModel(string value, Field field) {
        switch (field) {
            case Field.Name:
                if (value != ViewModel.Original.Name)
                    ViewModel.Updated.Name = value;
                break;
            case Field.Email:
                if (value != ViewModel.Original.Email)
                    ViewModel.Updated.Email = value;
                break;
            case Field.Phone:
                if (value != ViewModel.Original.Phone)
                    ViewModel.Updated.Phone = value;
                break;
            case Field.Fax:
                if (value != ViewModel.Original.Fax)
                    ViewModel.Updated.Fax = value;
                break;
            case Field.Notes:
                if (value != ViewModel.Original.Notes)
                    ViewModel.Updated.Notes = value;
                break;
            case Field.Secretary:
                if (value != ViewModel.Original.SecretaryName)
                    ViewModel.Updated.SecretaryName = value;
                break;
            case Field.SecretaryEmail:
                if (value != ViewModel.Original.SecretaryEmail)
                    ViewModel.Updated.SecretaryEmail = value;
                break;
        }
    }
}
