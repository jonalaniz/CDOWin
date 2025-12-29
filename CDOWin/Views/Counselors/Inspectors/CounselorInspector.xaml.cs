using CDO.Core.DTOs;
using CDOWin.ViewModels;
using CDOWin.Views.Counselors.Dialogs;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace CDOWin.Views.Counselors.Inspectors;

public sealed partial class CounselorInspector : Page {
    public CounselorsViewModel ViewModel { get; private set; } = null!;
    public CounselorInspector() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (CounselorsViewModel)e.Parameter;
        DataContext = ViewModel;
    }

    private async void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null)
            return;

        var updateVM = new CounselorUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Counselor");
        dialog.Content = new UpdateCounselor(updateVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            _ = ViewModel.UpdateCounselorAsync(updateVM.Updated);
        }
    }

    private void Delete_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {

    }
}
