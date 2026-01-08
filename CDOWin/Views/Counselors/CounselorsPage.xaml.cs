using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Counselors.Dialogs;
using CDOWin.Views.Counselors.Inspectors;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.ComponentModel;

namespace CDOWin.Views.Counselors;

public sealed partial class CounselorsPage : Page {

    // =========================
    // ViewModel
    // =========================
    public CounselorsViewModel ViewModel { get; } = AppServices.CounselorsViewModel;

    // =========================
    // Constructor
    // =========================
    public CounselorsPage() {
        InitializeComponent();
        InspectorFrame.Navigate(typeof(CounselorInspector));
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await ViewModel.LoadCounselorsAsync();
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

        PropertyChangedEventHandler handler = (_, args) => {
            if (args.PropertyName == nameof(createCounselorVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createCounselorVM.CanSave;
        };

        createCounselorVM.PropertyChanged += handler;

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            await createCounselorVM.CreateCounselorAsync();
            _ = ViewModel.LoadCounselorsAsync();
        }

        createCounselorVM.PropertyChanged -= handler;
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Counselor counselor) {
            _ = ViewModel.ReloadCounselorAsync(counselor.Id);
        }
    }
}
