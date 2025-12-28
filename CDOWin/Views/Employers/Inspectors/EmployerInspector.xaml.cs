using CDO.Core.DTOs;
using CDOWin.ViewModels;
using CDOWin.Views.Employers.Dialogs;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace CDOWin.Views.Employers.Inspectors;

public sealed partial class EmployerInspector : Page {
    public EmployersViewModel ViewModel { get; private set; } = null!;

    public EmployerInspector() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (EmployersViewModel)e.Parameter;
        DataContext = ViewModel;
    }

    private async void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null)
            return;

        var updateVM = new EmployerUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Employer");
        dialog.Content = new UpdateEmployer(updateVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            updateEmployer(updateVM.Updated);
        }
    }

    private void updateEmployer(EmployerDTO update) {
        _ = ViewModel.UpdateEmployerAsync(update);
    }
}
