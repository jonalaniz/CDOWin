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
    public CounselorsViewModel ViewModel { get; private set; } = AppServices.CounselorsViewModel;

    // =========================
    // Constructor
    // =========================
    public CounselorInspector() {
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null)
            return;

        var updateVM = new CounselorUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Counselor");
        dialog.Content = new UpdateCounselor(updateVM);

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await ViewModel.UpdateCounselorAsync(updateVM.Updated);
        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }
    }

    private void SA_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {

    }
}
