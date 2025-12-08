using CDO.Core.DTOs;
using CDOWin.ViewModels;
using CDOWin.Views.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;

namespace CDOWin.Views;

public sealed partial class ClientViewPage : Page {
    public ClientsViewModel? ViewModel { get; private set; }
    public ClientViewPage() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (ClientsViewModel)e.Parameter;
        DataContext = ViewModel;
    }

    // Click Events
    private void OpenDocuments_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        Process.Start("explorer.exe", $"{ViewModel.SelectedClient?.documentsFolderPath}");
    }

    private void Checkbox_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (sender is CheckBox checkBox && checkBox.Tag is CheckboxTag tag) {
            var isChecked = checkBox.IsChecked;
            Debug.WriteLine($"Checkbox: {tag}");
        }
    }

    private async void EditButton_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (sender is Button button && button.Tag is ClientEditType tag) {

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.PrimaryButtonText = "Save";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;

            var updateVM = new ClientUpdateViewModel(ViewModel.SelectedClient);

            switch (tag) {
                case ClientEditType.Personal:
                    dialog.Title = "Edit Personal Information";
                    dialog.Content = new UpdatePersonalInformation(updateVM);
                    break;
                case ClientEditType.Case:
                    dialog.Title = "Edit Case Information";
                    dialog.Content = new UpdateCaseInformation(updateVM);
                    break;
                case ClientEditType.Employment:
                    dialog.Title = "Edit Employment Profile";
                    dialog.Content = new UpdateEmploymentProfile(updateVM);
                    break;
                case ClientEditType.Arrangements:
                    dialog.Title = "Edit Arrangements";
                    dialog.Content = new UpdateArrangements(updateVM);
                    break;
                case ClientEditType.Contact:
                    dialog.Title = "Edit Contact Information";
                    dialog.Content = new UpdateContacts(updateVM);
                    break;
            }

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary) {
                updateClient(updateVM.UpdatedClient);
            }
        }
    }

    // Utility Methods

    private void UpdateCheckbox(CheckboxTag tag, bool isChecked) {
        var updateVM = new ClientUpdateViewModel(ViewModel.SelectedClient);
        updateVM.UpdateCheckbox(tag, isChecked);
        updateClient(updateVM.UpdatedClient);
    }

    private void updateClient(UpdateClientDTO update) {
        ViewModel.UpdateClient(update);
    }
}
