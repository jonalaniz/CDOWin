using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

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
        DataContext = viewModel;
        InitializeComponent();
    }

    // =========================
    // Property Change Methods
    // =========================
    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        if (sender is LabeledTextBox pair
            && pair.innerTextBox.Text.NormalizeString() is string value
            && !string.IsNullOrWhiteSpace(value)
            && pair.Tag is Field field)
            ViewModel.UpdateField(field, value);
    }
}
