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
    private void OpenDocuments_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        Debug.WriteLine($"Open {ViewModel.SelectedClient?.documentsFolderPath}");
        Process.Start("explorer.exe", $"{ViewModel.SelectedClient?.documentsFolderPath}");
    }

    private void Checkbox_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (sender is CheckBox checkBox) {
            string? checkboxName = checkBox.Tag?.ToString();
            if (string.IsNullOrEmpty(checkboxName))
                return;
            Debug.WriteLine($"Checkbox: {checkboxName} was checked with value: {checkBox.IsChecked}");
            // resumeRequired
            // resumeCompleted
            // videoInterviewRequired
            // videoInterviewCompleted
            // releasesCompleted
            // orientationCompleted
            // dataSheetCompleted
            // elevatorSpeechCompleted
        }
    }

    private async void EditButton_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (sender is Button button) {
            string? editType = button.Tag?.ToString();
            if (string.IsNullOrEmpty(editType))
                return;

            Debug.WriteLine($"{editType} edit button pressed.");

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.PrimaryButtonText = "Save";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;

            var updateViewModel = new ClientUpdateViewModel(ViewModel.SelectedClient);

            switch (editType) {
                case "personalInformation":
                    dialog.Title = "Edit Personal Information";
                    dialog.Content = new UpdatePersonalInformation(updateViewModel);
                    break;
                case "caseInformation":
                    dialog.Title = "Edit Case Information";
                    dialog.Content = new UpdateCaseInformation(updateViewModel);
                    break;
                case "employmentProfile":
                    dialog.Title = "Edit Employment Profile";
                    dialog.Content = new UpdateEmploymentProfile(updateViewModel);
                    break;
                case "arrangements":
                    dialog.Title = "Edit Arrangements";
                    dialog.Content = new UpdateArrangements(updateViewModel);
                    break;
                case "contactInformation":
                    dialog.Title = "Edit Contact Information";
                    dialog.Content = new UpdateContacts(updateViewModel);
                    break;

                default: break;
            }

            var result = await dialog.ShowAsync();
            Debug.WriteLine(result);

            if (result == ContentDialogResult.Primary) {
                updateViewModel.UpdateContact();
            }

        }
    }
}
