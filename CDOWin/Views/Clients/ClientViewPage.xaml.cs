using CDO.Core.DTOs;
using CDO.Core.Models;
using CDOWin.Composers;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Dialogs;
using CDOWin.Views.Placements.Dialogs;
using CDOWin.Views.ServiceAuthorizations.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

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
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        if (ViewModel.Selected == null) return;
        _ = ViewModel.ReloadClientAsync();
    }

    // =========================
    // Click Handlers
    // =========================

    // Header
    private async void TextBlock_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e) {
        if (ViewModel.Selected == null) return;

        var old = Header.Text;
        var data = new DataPackage();
        data.SetText($"{ViewModel.Selected.FirstName} {ViewModel.Selected.LastName}");

        Clipboard.SetContent(data);
        Header.Text = "Copied!";
        await Task.Delay(650);
        Header.Text = old;
    }

    // Documents
    private void OpenDocuments_Clicked(object sender, RoutedEventArgs e) {
        Process.Start("explorer.exe", $"{ViewModel.Selected?.DocumentsFolderPath}");
    }

    // Reminders
    private async void CreateReminder_ClickAsync(object sender, RoutedEventArgs e) {
        if (sender is not Button || ViewModel.Selected == null) return;

        // Initialize our dialog/vm/page
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"Create Reminder for {ViewModel.Selected.NameAndID}");
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

        if (result != ContentDialogResult.Primary) return;

        var reminderResult = await createReminderVM.CreateReminderAsync();
        if (!reminderResult.IsSuccess) {
            ErrorHandler.Handle(reminderResult, this.XamlRoot);
            return;
        }

        _ = ViewModel.ReloadClientAsync();
        ViewModel.NotifyNewClientCreated();
    }

    private async void ReminderFlyoutItem_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item
            && item.Tag is ReminderMenuItem reminderItem
            && ViewModel.Selected != null) {
            var newReminderVM = AppServices.CreateReminderViewModel(ViewModel.Selected.Id);
            newReminderVM.Description = reminderItem.Description;

            var dateOffset = DateTimeOffset.Now.AddDays(reminderItem.Days);
            newReminderVM.Date = dateOffset.Date.ToUniversalTime();

            var reminderResult = await newReminderVM.CreateReminderAsync();
            if (!reminderResult.IsSuccess) {
                ErrorHandler.Handle(reminderResult, this.XamlRoot);
                return;
            }

            _ = ViewModel.ReloadClientAsync();
            ViewModel.NotifyNewClientCreated();
        }
    }

    // SAs
    private async void CreateSA_Click(object sender, RoutedEventArgs e) {
        if (ViewModel.Selected == null) return;

        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"New Service Authorization for {ViewModel.Selected.NameAndID}");
        var createSAVM = AppServices.CreateServiceAuthorizationsViewModel(ViewModel.Selected);
        var createSAPage = new CreateServiceAuthorization(createSAVM);
        dialog.Content = createSAPage;
        dialog.IsPrimaryButtonEnabled = createSAVM.CanSave;

        PropertyChangedEventHandler handler = (_, args) => {
            if (args.PropertyName == nameof(createSAVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createSAVM.CanSave;
        };

        createSAVM.PropertyChanged += handler;

        var result = await dialog.ShowAsync();
        createSAVM.PropertyChanged -= handler;

        if (result != ContentDialogResult.Primary) return;
        var sAResult = await createSAVM.CreateSAAsync();

        if (!sAResult.IsSuccess) {
            ErrorHandler.Handle(sAResult, this.XamlRoot);
            return;
        }

        _ = ViewModel.ReloadClientAsync();
    }

    private async void SA_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not string id) { return; }
        var sa = ViewModel.Selected?.Pos?.FirstOrDefault(c => c.Id == id);

        if (sa == null) { return; }
        var updateSAVM = new ServiceAuthorizationUpdateViewModel(sa);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Service Authorization");
        dialog.SecondaryButtonText = "Export";
        dialog.Content = new UpdateSA(updateSAVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            var updateResult = await updateSAVM.UpdateSAAsync();
            if (!updateResult.IsSuccess) {
                ErrorHandler.Handle(updateResult, this.XamlRoot);
                return;
            }
            _ = ViewModel.ReloadClientAsync();
        } else if (result == ContentDialogResult.Secondary) {
            var export = Invoice.InjectClient(sa, ViewModel.Selected!);
            var composer = new ServiceAuthorizationComposer(export);
            var composerResult = await composer.Compose();

            if (composerResult.IsSuccess) return;
            ErrorHandler.Handle(composerResult, this.XamlRoot);
        }
    }

    // Placements
    private async void CreatePlacement_Click(object sender, RoutedEventArgs e) {
        if (ViewModel.Selected == null) return;

        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"New Placement for {ViewModel.Selected.NameAndID}");
        var createPlacementVM = AppServices.CreatePlacementViewMdoel(ViewModel.Selected);
        var createPage = new CreatePlacements(createPlacementVM);
        dialog.Content = createPage;
        dialog.IsPrimaryButtonEnabled = createPlacementVM.CanSave;

        PropertyChangedEventHandler handler = (_, args) => {
            if (args.PropertyName == nameof(createPlacementVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createPlacementVM.CanSave;
        };

        createPlacementVM.PropertyChanged += handler;

        var result = await dialog.ShowAsync();
        createPlacementVM.PropertyChanged += handler;

        if (result != ContentDialogResult.Primary) return;
        var placementResult = await createPlacementVM.CreatePlacementAsync();

        if (!placementResult.IsSuccess) {
            ErrorHandler.Handle(placementResult, this.XamlRoot);
            return;
        }

        _ = ViewModel.ReloadClientAsync();
    }

    private async void Placement_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not string id) { return; }
        var placement = ViewModel.Selected?.Placements?.FirstOrDefault(c => c.Id == id);

        if (placement == null) { return; }
        var updatePlacementVM = new PlacementUpdateViewModel(placement);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Placement");
        // dialog.SecondaryButtonText = "Export";
        dialog.Content = new UpdatePlacement(updatePlacementVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            var updateResult = await updatePlacementVM.UpdatePlacementAsync();
            if (!updateResult.IsSuccess) {
                ErrorHandler.Handle(updateResult, this.XamlRoot);
                return;
            }
            _ = ViewModel.ReloadClientAsync();
        }
    }



    private void Checkbox_Clicked(object sender, RoutedEventArgs e) {
        if (sender is CheckBox checkBox && checkBox.Tag is CheckboxTag tag) {
            var isChecked = checkBox.IsChecked;
            _ = UpdateCheckboxAsync(tag, isChecked ?? false);
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
                case ClientEditType.Conditions:
                    dialog.Title = "Edit Conditions";
                    dialog.Content = new UpdateArrangements(updateVM);
                    break;
                case ClientEditType.Contact:
                    dialog.Title = "Edit Contact Information";
                    dialog.Content = new UpdateContacts(updateVM);
                    break;
            }

            var result = await dialog.ShowAsync();

            if (result != ContentDialogResult.Primary) return;
            _ = UpdateClient(updateVM.UpdatedClient);
        }
    }

    // =========================
    // Utility Methods
    // =========================
    private async Task UpdateCheckboxAsync(CheckboxTag tag, bool isChecked) {
        if (ViewModel.Selected == null) return;

        var updateVM = new ClientUpdateViewModel(ViewModel.Selected);
        updateVM.UpdateCheckbox(tag, isChecked);

        _ = UpdateClient(updateVM.UpdatedClient);
    }

    private async Task UpdateClient(UpdateClientDTO update) {
        var result = await ViewModel.UpdateClientAsync(update);
        if (!result.IsSuccess) {
            ErrorHandler.Handle(result, this.XamlRoot);
            return;
        }
    }
}
