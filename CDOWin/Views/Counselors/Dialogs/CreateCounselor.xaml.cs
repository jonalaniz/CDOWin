using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Counselors.Dialogs;

public sealed partial class CreateCounselor : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreateCounselorViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public CreateCounselor(CreateCounselorViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
    }

    // =========================
    // Property Change Methods
    // =========================
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not Field field)
            return;

        var text = textbox.Text.NormalizeString();
        if (text == null) return;
        _viewModel.UpdateField(field, text);
    }

    private void CaseLoad_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {
        var value = (int)sender.Value;
        _viewModel.CaseLoadId = value;
    }
}
