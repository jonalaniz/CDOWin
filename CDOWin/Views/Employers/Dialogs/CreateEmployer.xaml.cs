using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Employers.Dialogs;

public sealed partial class CreateEmployer : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreateEmployerViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public CreateEmployer(CreateEmployerViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
    }

    // =========================
    // Property Change Methods
    // =========================
    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        if (sender is not FrameworkElement fe || fe.Tag is not Field field)
            return;

        string? text = sender switch {
            LabeledTextBox p => p.innerTextBox.Text,
            LabeledMultiLinePair p => p.innerTextBox.Text,
            _ => null
        };

        text = text?.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        ViewModel.UpdateField(field, text);
    }
}
