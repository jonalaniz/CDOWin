using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Counselors.Dialogs;

public sealed partial class CreateCounselor : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreateCounselorViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public CreateCounselor(CreateCounselorViewModel viewModel) {
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

        ViewModel.UpdateField(field, text);
    }
}
