using CDO.UI.Shared.Factories;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Counselors.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.Counselors.Inspectors;

public sealed partial class CounselorInspector : Page {

    // =========================
    // ViewModel
    // =========================
    private readonly CounselorsViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public CounselorInspector() {
        _viewModel = AppServices.CounselorsViewModel;
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (_viewModel == null || _viewModel.Selected == null)
            return;

        var updateVM = new CounselorUpdateViewModel(_viewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Counselor");
        dialog.Content = new UpdateCounselor(updateVM);

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await _viewModel.UpdateCounselorAsync(updateVM.Updated);
        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }

        _ = _viewModel.LoadSelectedCounselorAsync(_viewModel.Selected.Id);
    }

    private async void SA_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {

    }
}
