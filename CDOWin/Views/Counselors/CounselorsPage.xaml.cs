using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Counselors.Dialogs;
using CDOWin.Views.Counselors.Inspectors;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.Counselors;

public sealed partial class CounselorsPage : Page {

    // =========================
    // ViewModel
    // =========================
    public CounselorsViewModel ViewModel { get; }

    // =========================
    // Constructor
    // =========================
    public CounselorsPage() {
        InitializeComponent();
        ViewModel = AppServices.CounselorsViewModel;
        DataContext = ViewModel;
        InspectorFrame.Navigate(typeof(CounselorInspector), ViewModel);
    }

    // =========================
    // Click Handlers
    // =========================
    private async void NewCounselor_ClickAsync(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"Create Counselor");
        var createCounselorVM = AppServices.CreateCounselorViewModel();
        var createCounselorPage = new CreateCounselor(createCounselorVM);
        dialog.Content = createCounselorPage;
        dialog.IsPrimaryButtonEnabled = createCounselorVM.CanSave;

        createCounselorVM.PropertyChanged += (_, args) => {
            if (args.PropertyName == nameof(createCounselorVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createCounselorVM.CanSave;
        };

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            await createCounselorVM.CreateCounselorAsync();
            _ = ViewModel.LoadCounselorsAsync();
        }
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Counselor counselor) {
            _ = ViewModel.ReloadCounselorAsync(counselor.id);
        }
    }
}
