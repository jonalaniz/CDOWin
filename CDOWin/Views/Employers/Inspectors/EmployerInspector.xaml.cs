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
    public EmployersViewModel ViewModel { get; private set; } = AppServices.EmployersViewModel;

    // =========================
    // Constructor
    // =========================
    public EmployerInspector() {
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null)
            return;

        var updateVM = new EmployerUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit EmployerName");
        dialog.Content = new UpdateEmployer(updateVM);

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await ViewModel.UpdateEmployerAsync(updateVM.Updated);
        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }
    }
}
