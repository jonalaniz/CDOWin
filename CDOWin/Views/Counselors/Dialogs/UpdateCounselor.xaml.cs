using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Counselors.Dialogs;

public sealed partial class UpdateCounselor : Page {
    public CounselorUpdateViewModel ViewModel;

    // Constructor
    public UpdateCounselor(CounselorUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.Original;
        InitializeComponent();
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        string? originalValue = null;
        string? updatedValue = null;
        Field? field = null;

        if (sender is LabeledMultiLinePair multiLinePair) {
            originalValue = multiLinePair.Value.NormalizeString();
            updatedValue = multiLinePair.innerTextBox.Text.NormalizeString();
            field = (Field)multiLinePair.Tag;
        } else if (sender is LabeledTextBox multiLineTextBox) {
            originalValue = multiLineTextBox.Value.NormalizeString();
            updatedValue = multiLineTextBox.innerTextBox.Text.NormalizeString();
            field = (Field)multiLineTextBox.Tag;
        }

        if (field == null || originalValue == updatedValue || updatedValue == null)
            return;

        UpdateModel(updatedValue, field.Value);
    }

    private void UpdateModel(string value, Field field) {
        switch (field) {
            case Field.Name:
                ViewModel.Updated.name = value;
                break;
            case Field.Email:
                ViewModel.Updated.email = value;
                break;
            case Field.Phone:
                ViewModel.Updated.phone = value;
                break;
            case Field.Fax:
                ViewModel.Updated.fax = value;
                break;
            case Field.Notes:
                ViewModel.Updated.notes = value;
                break;
            case Field.Secretary:
                ViewModel.Updated.secretaryName = value;
                break;
            case Field.SecretaryEmail:
                ViewModel.Updated.secretaryEmail = value;
                break;
        }
    }
}
