using CDO.Core.DTOs;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Dialogs;
using CDOWin.Views.ServiceAuthorizations.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CDOWin.Views.Clients;

public sealed partial class ClientViewPage : Page {

    // =========================
    // ViewModel
    // =========================
    public ClientsViewModel ViewModel { get; } = AppServices.ClientsViewModel;

    // =========================
    // Constructor
    // =========================
    public ClientViewPage() {
        InitializeComponent();
        BuildRemindersFlyout();
    }

    // =========================
    // UI Setup
    // =========================
    private void BuildRemindersFlyout() {
        var flyout = new MenuFlyout();

        foreach (var reminderItem in ReminderMenuItem.AllItems()) {
            var item = new MenuFlyoutItem {
                Text = reminderItem.ToString(),
                Tag = reminderItem
            };

            item.Click += ReminderFlyoutItem_Click;
            flyout.Items.Add(item);
        }

        RemindersSplitButton.Flyout = flyout;
    }

    // =========================
    // Click Handlers
    // =========================
    private void OpenDocuments_Clicked(object sender, RoutedEventArgs e) {
        Process.Start("explorer.exe", $"{ViewModel.Selected?.DocumentsFolderPath}");
    }

    private async void CreateReminder_ClickAsync(SplitButton sender, SplitButtonClickEventArgs e) {
        if (ViewModel.Selected == null) return;

        // Initialize our dialog/vm/page
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"Create Reminder for {ViewModel.Selected.Name}");
        var createReminderVM = AppServices.CreateReminderViewModel(ViewModel.Selected.Id);
        var createReminderPage = new CreateReminder(createReminderVM);

        // Set the content
        dialog.Content = createReminderPage;

        // Set the button State
        dialog.IsPrimaryButtonEnabled = createReminderVM.CanSave;

        // Keep button State in sync with ViewModel
        createReminderVM.PropertyChanged += (_, args) => {
            if (args.PropertyName == nameof(createReminderVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createReminderVM.CanSave;
        };

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            await createReminderVM.CreateReminderAsync();
            _ = ViewModel.ReloadClientAsync();
            ViewModel.NotifyNewClientCreated();
        }

    }

    private async void CreateSA_Click(object sender, RoutedEventArgs e) {
        if (ViewModel.Selected == null) return;

        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"New Service Authorization for {ViewModel.Selected.Name}");
        var createSAVM = AppServices.CreateServiceAuthorizationsViewModel(ViewModel.Selected);
        var createSAPage = new CreateServiceAuthorization(createSAVM, ViewModel.Selected.Id);
        dialog.Content = createSAPage;
        dialog.IsPrimaryButtonEnabled = createSAVM.CanSave;

        PropertyChangedEventHandler handler = (_, args) => {
            if (args.PropertyName == nameof(createSAVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createSAVM.CanSave;
        };

        createSAVM.PropertyChanged += handler;

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            await createSAVM.CreateSAAsync();
            _ = ViewModel.ReloadClientAsync();
        }

        createSAVM.PropertyChanged -= handler;
    }

    private void SA_Click(object sender, RoutedEventArgs e) {
        if (sender is Button button && button.Tag is string id) {
            ViewModel.SASelected(id);
            AppServices.Navigation.Navigate(CDOFrame.ServiceAuthorizations);
        }
    }

    private void Placement_Click(object sender, RoutedEventArgs e) {
        if (sender is Button button && button.Tag is string id) {
            ViewModel.PlacementSelected(id);
            AppServices.Navigation.Navigate(CDOFrame.Placements);
        }
    }

    private async void ReminderFlyoutItem_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item
            && item.Tag is ReminderMenuItem reminderItem
            && ViewModel.Selected != null) {
            var newReminderVM = AppServices.CreateReminderViewModel(ViewModel.Selected.Id);
            newReminderVM.Description = reminderItem.Description;

            var dateOffset = DateTimeOffset.Now.AddDays(reminderItem.Days);
            newReminderVM.Date = dateOffset.Date.ToUniversalTime();

            await newReminderVM.CreateReminderAsync();
            _ = ViewModel.ReloadClientAsync();
            ViewModel.NotifyNewClientCreated();
        }
    }

    private void Checkbox_Clicked(object sender, RoutedEventArgs e) {
        if (sender is CheckBox checkBox && checkBox.Tag is CheckboxTag tag) {
            var isChecked = checkBox.IsChecked;
            UpdateCheckbox(tag, isChecked ?? false);
        }
    }

    private async void EditButton_Clicked(object sender, RoutedEventArgs e) {
        if (sender is Button button && button.Tag is ClientEditType tag && ViewModel.Selected != null) {

            var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "");
            var updateVM = new ClientUpdateViewModel(ViewModel.Selected);

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
                Debug.WriteLine(updateVM.UpdatedClient.Benefits);
                UpdateClient(updateVM.UpdatedClient);
            }
        }
    }

    // =========================
    // Utility Methods
    // =========================
    private void UpdateCheckbox(CheckboxTag tag, bool isChecked) {
        if (ViewModel.Selected == null) return;
        var updateVM = new ClientUpdateViewModel(ViewModel.Selected);
        updateVM.UpdateCheckbox(tag, isChecked);
        UpdateClient(updateVM.UpdatedClient);
    }

    private void UpdateClient(UpdateClientDTO update) {
        _ = ViewModel.UpdateClientAsync(update);
    }
}
