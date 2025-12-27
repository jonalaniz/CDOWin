using CDO.Core.DTOs;
using CDO.Core.Models;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;

namespace CDOWin.Views.Clients;

public sealed partial class ClientViewPage : Page {
    public ClientsViewModel ViewModel { get; private set; } = null!;
    public ClientViewPage() {
        InitializeComponent();
        BuildRemindersFlyout();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (ClientsViewModel)e.Parameter;
        DataContext = ViewModel;
    }

    // UI Setup
    private void BuildRemindersFlyout() {
        var flyout = new MenuFlyout();

        foreach (var reminderItem in ReminderMenuItem.AllItems()) {
            var item = new MenuFlyoutItem {
                Text = reminderItem.ToString(),
                Tag = reminderItem.Value
            };

            item.Click += ReminderFlyoutItem_Click;
            flyout.Items.Add(item);
        }

        RemindersSplitButton.Flyout = flyout;
    }

    // Click Events
    private void OpenDocuments_Clicked(object sender, RoutedEventArgs e) {
        Process.Start("explorer.exe", $"{ViewModel.SelectedClient?.documentsFolderPath}");
    }

    private void NewReminder_Click(SplitButton sender, SplitButtonClickEventArgs e) {
        Debug.WriteLine("New Reminder Button Clicked");
    }

    private void ReminderFlyoutItem_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item) {
            Debug.WriteLine($"Item: {item.Tag.ToString()} clicked");
        }
    }

    private void Checkbox_Clicked(object sender, RoutedEventArgs e) {
        if (sender is CheckBox checkBox && checkBox.Tag is CheckboxTag tag) {
            var isChecked = checkBox.IsChecked;
        }
    }

    private async void EditButton_Clicked(object sender, RoutedEventArgs e) {
        if (sender is Button button && button.Tag is ClientEditType tag && ViewModel.SelectedClient != null) {

            var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "");
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
        if (ViewModel.SelectedClient == null) return;
        var updateVM = new ClientUpdateViewModel(ViewModel.SelectedClient);
        updateVM.UpdateCheckbox(tag, isChecked);
        updateClient(updateVM.UpdatedClient);
    }

    private void updateClient(UpdateClientDTO update) {
        _ = ViewModel.UpdateClient(update);
    }
}
