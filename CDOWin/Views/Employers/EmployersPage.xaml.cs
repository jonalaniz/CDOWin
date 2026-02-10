using CDO.Core.DTOs.Employers;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Employers.Dialogs;
using CDOWin.Views.Employers.Inspectors;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.ComponentModel;

namespace CDOWin.Views.Employers;

public sealed partial class EmployersPage : Page {

    // =========================
    // ViewModel
    // =========================
    private EmployersViewModel ViewModel { get; } = AppServices.EmployersViewModel;

    // =========================
    // Constructor
    // =========================
    public EmployersPage() {
        InitializeComponent();
        InspectorFrame.Navigate(typeof(EmployerInspector));
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await ViewModel.LoadEmployerSummariesAsync();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void NewEmployer_ClickAsync(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"Create EmployerName");
        var createEmployerVM = AppServices.CreateEmployerViewModel();
        var createEmployerPage = new CreateEmployer(createEmployerVM);
        dialog.Content = createEmployerPage;
        dialog.IsPrimaryButtonEnabled = createEmployerVM.CanSave;

        PropertyChangedEventHandler handler = (_, args) => {
            if (args.PropertyName == nameof(createEmployerVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createEmployerVM.CanSave;
        };

        createEmployerVM.PropertyChanged += handler;

        var result = await dialog.ShowAsync();
        createEmployerVM.PropertyChanged -= handler;

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await createEmployerVM.CreateEmployerAsync();
        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }

        await ViewModel.LoadEmployerSummariesAsync(force: true);
        _ = ViewModel.LoadSelectedEmployerAsync(updateResult.Value!.Id);
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is EmployerSummary employer) {
            _ = ViewModel.LoadSelectedEmployerAsync(employer.Id);
        }
    }
}
