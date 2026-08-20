using CDO.UI.Shared.Factories;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Employers.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.Employers.Inspectors;

public sealed partial class EmployerInspector : Page {

    // =========================
    // ViewModel
    // =========================
    private readonly EmployersViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public EmployerInspector() {
        _viewModel = AppServices.EmployersViewModel;
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (_viewModel == null || _viewModel.Selected == null)
            return;

        var updateVM = new EmployerUpdateViewModel(_viewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Employer");
        dialog.Content = new UpdateEmployer(updateVM);

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await _viewModel.UpdateEmployerAsync(updateVM.Updated);
        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }
    }
}
