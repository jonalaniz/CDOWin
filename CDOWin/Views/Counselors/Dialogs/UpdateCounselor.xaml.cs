using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Counselors.Dialogs;

public sealed partial class UpdateCounselor : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CounselorUpdateViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateCounselor(CounselorUpdateViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
        SetupNumberBox();
    }

    // =========================
    // UI Methods
    // =========================
    private void SetupNumberBox() {
        if (_viewModel.Original.CaseLoadId == null) return;
        CaseLoad_Numberbox.Value = (double)_viewModel.Original.CaseLoadId;
    }

    // =========================
    // Property Change Methods
    // =========================
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not Field field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;

        UpdateModel(text, field);
    }

    private void CaseLoad_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {
        var value = (int)sender.Value;
        _viewModel.Updated.CaseLoadID = value;
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateModel(string value, Field field) {
        switch (field) {
            case Field.Name:
                if (value != _viewModel.Original.Name || !string.IsNullOrWhiteSpace(value))
                    _viewModel.Updated.Name = value;
                break;
            case Field.Email:
                if (value != _viewModel.Original.Email)
                    _viewModel.Updated.Email = value;
                break;
            case Field.Phone:
                if (value != _viewModel.Original.Phone)
                    _viewModel.Updated.Phone = value;
                break;
            case Field.Fax:
                if (value != _viewModel.Original.Fax)
                    _viewModel.Updated.Fax = value;
                break;
            case Field.Notes:
                if (value != _viewModel.Original.Notes)
                    _viewModel.Updated.Notes = value;
                break;
            case Field.Secretary:
                if (value != _viewModel.Original.SecretaryName)
                    _viewModel.Updated.SecretaryName = value;
                break;
            case Field.SecretaryEmail:
                if (value != _viewModel.Original.SecretaryEmail)
                    _viewModel.Updated.SecretaryEmail = value;
                break;
        }
    }
}
