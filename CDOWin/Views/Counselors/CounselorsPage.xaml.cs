using CDO.Core.DTOs;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Counselors.Dialogs;
using CDOWin.Views.Counselors.Inspectors;
using Microsoft.UI.Xaml;
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
        await ViewModel.LoadCounselorSummariesAsync();
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
        createCounselorVM.PropertyChanged -= handler;

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await createCounselorVM.CreateCounselorAsync();
        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }

        _ = ViewModel.LoadCounselorSummariesAsync();
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is CounselorSummaryDTO counselor) {
            _ = ViewModel.LoadSelectedCounselorAsync(counselor.Id);
        }
    }

    private void GoToClient_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        ViewModel.RequestClient(id);
    }
}
