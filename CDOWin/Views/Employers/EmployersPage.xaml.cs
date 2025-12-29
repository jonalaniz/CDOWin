using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Employers.Dialogs;
using CDOWin.Views.Employers.Inspectors;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace CDOWin.Views.Employers;

public sealed partial class EmployersPage : Page {

    // =========================
    // ViewModel
    // =========================
    private EmployersViewModel ViewModel { get; }

    // =========================
    // Constructor
    // =========================
    public EmployersPage() {
        InitializeComponent();
        ViewModel = AppServices.EmployersViewModel;
        DataContext = ViewModel;
        InspectorFrame.Navigate(typeof(EmployerInspector), ViewModel);
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

        createEmployerVM.PropertyChanged += (_, args) => {
            if (args.PropertyName == nameof(createEmployerVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createEmployerVM.CanSave;
        };

        var result = await dialog.ShowAsync();

        if(result == ContentDialogResult.Primary) {
            await createEmployerVM.CreateEmployerAsync();
            _ = ViewModel.LoadEmployersAsync();
        }
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Employer employer) {
            _ = ViewModel.ReloadEmployerAsync(employer.id);
        }
    }
}
