using CDO.Core.Models;
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
        await ViewModel.LoadEmployersAsync();
    }

    // =========================
    // Click Handlers
    // =========================
    private async void NewEmployer_ClickAsync(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"Create Employer");
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

        if (result == ContentDialogResult.Primary) {
            await createEmployerVM.CreateEmployerAsync();
            _ = ViewModel.LoadEmployersAsync();
        }

        createEmployerVM.PropertyChanged -= handler;
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Employer employer) {
            _ = ViewModel.ReloadEmployerAsync(employer.Id);
        }
    }
}
