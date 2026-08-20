using CDO.Core.Constants;
using CDO.Core.DTOs.Clients;
using CDO.UI.Shared.Factories;
using CDOWin.Composers;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace CDOWin.Views.Clients;

public sealed partial class ClientViewPage : Page {

    // =========================
    // ViewModel
    // =========================
    private readonly ClientsViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public ClientViewPage() {
        _viewModel = AppServices.ClientsViewModel;
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        if (_viewModel.Selected == null) return;
        _ = _viewModel.ReloadClientAsync();
    }

    // =========================
    // Click Handlers
    // =========================

    // Header
    private async void TextBlock_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e) {
        if (_viewModel.Selected == null) return;

        var old = Header.Text;
        var data = new DataPackage();
        data.SetText($"{_viewModel.Selected.FirstName} {_viewModel.Selected.LastName}");

        Clipboard.SetContent(data);
        Header.Text = "Copied!";
        await Task.Delay(650);
        Header.Text = old;
    }

    // Documents
    private void OpenDocuments_Clicked(object sender, RoutedEventArgs e) {
        if (_viewModel.Selected?.DocumentsFolderPath is not string path || !Directory.Exists(path)) {
            _ = ShowMessage(ClientPageMessageType.DocumentsFolderMissing, false);
            return;
        }

        Process.Start("explorer.exe", $"{path}");
    }

    // Reminders
    private async void CreateReminder_ClickAsync(object sender, RoutedEventArgs e) {
        if (sender is not Button || _viewModel.Selected == null) return;

        // Initialize our dialog/vm/page
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"Create Reminder for {_viewModel.Selected.NameAndID}");
        var createReminderVM = AppServices.CreateReminderViewModel(_viewModel.Selected.Id);
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
        _ = ShowMessage(ClientPageMessageType.CreatedReminder, reminderResult.IsSuccess);

        if (!reminderResult.IsSuccess) return;

        _viewModel.NotifyNewReminderCreated();
        _ = _viewModel.ReloadClientAsync();
    }

    // Counselors
    private void GoToCounselor_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;
        _viewModel.RequestCounselor(id);
    }

    // SAs
    private async void CreateSA_Click(object sender, RoutedEventArgs e) {
        if (_viewModel.Selected == null) return;

        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"New Service Authorization for {_viewModel.Selected.NameAndID}");
        var createSAVM = AppServices.CreateServiceAuthorizationsViewModel(_viewModel.Selected);
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
        _ = ShowMessage(ClientPageMessageType.CreatedSA, sAResult.IsSuccess);

        if (!sAResult.IsSuccess) return;

        _ = _viewModel.ReloadClientAsync();
    }

    private async void SA_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) { return; }
        var invoice = _viewModel.Selected?.Sas?.FirstOrDefault(i => i.Id == id);

        if (invoice == null) { return; }
        var updateSAVM = new ServiceAuthorizationUpdateViewModel(invoice);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Service Authorization");
        dialog.SecondaryButtonText = "Export";
        dialog.Content = new UpdateSA(updateSAVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            var updateResult = await updateSAVM.UpdateSAAsync();
            _ = ShowMessage(ClientPageMessageType.UpdatedSA, updateResult.IsSuccess);
            if (!updateResult.IsSuccess) return;
            _ = _viewModel.ReloadClientAsync();
        } else if (result == ContentDialogResult.Secondary) {
            var composer = new ServiceAuthorizationComposer(invoice);
            var composerResult = await composer.Compose();

            _ = ShowMessage(ClientPageMessageType.ExportedSA, composerResult.IsSuccess);
            if (composerResult.IsSuccess) return;
        }
    }

    // Placements
    private async void CreatePlacement_Click(object sender, RoutedEventArgs e) {
        if (_viewModel.Selected == null) return;

        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, $"New Placement for {_viewModel.Selected.NameAndID}");
        var createPlacementVM = AppServices.CreatePlacementViewMdoel(_viewModel.Selected);
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
        _ = ShowMessage(ClientPageMessageType.CreatedPlacement, placementResult.IsSuccess);

        if (!placementResult.IsSuccess) return;

        _ = AppServices.DataCoordinator.GetPlacementSummariesAsync(force: true);
        _ = AppServices.DataCoordinator.GetEmployerSummariesAsync(force: true);
        _ = _viewModel.ReloadClientAsync();
    }

    private async void Placement_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) { return; }
        var placement = _viewModel.Selected?.Placements?.FirstOrDefault(c => c.Id == id);

        if (placement == null) { return; }
        var updatePlacementVM = new PlacementUpdateViewModel(placement);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Placement");
        dialog.Content = new UpdatePlacement(updatePlacementVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            var updateResult = await updatePlacementVM.UpdatePlacementAsync();
            _ = ShowMessage(ClientPageMessageType.UpdatedPlacement, updateResult.IsSuccess);
            if (!updateResult.IsSuccess) return;
            _ = _viewModel.ReloadClientAsync();
        }
    }

    private void GoToPlacement_Click(object sender, RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) { return; }
        _viewModel.RequestPlacement(id);
    }

    private void Checkbox_Clicked(object sender, RoutedEventArgs e) {
        if (sender is not CheckBox checkBox || checkBox.Tag is not CheckboxTag tag) return;
        var isChecked = checkBox.IsChecked;
        _ = UpdateCheckboxAsync(tag, isChecked ?? false);
    }

    private async void ToggleActive_Clicked(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem || _viewModel.Selected?.Active is not bool isActive) return;

        if (isActive) {
            var dialog = DialogFactory.MarkInactiveDialog(this.XamlRoot, "Mark Client Inactive?");
            dialog.Content = "Marking this client inactive will remove all existing reminders. This action cannot be undone.";

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
                _ = ToggleActiveAsync(isActive);
        } else {
            _ = ToggleActiveAsync(isActive);
        }
    }

    private async void ToggleTTW_Clicked(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem || _viewModel.Selected?.TTW is not bool isTTW) return;
        _ = ToggleTTWAsync(isTTW);
    }

    private async void Delete_Clicked(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem || _viewModel.Selected == null) return;

        var dialog = DialogFactory.DeleteDialog(this.XamlRoot, $"Delete {_viewModel.Selected.FormattedName}?");
        dialog.Content = "Deleting this client will remove all existing reminders. This action cannot be undone.";

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary) {
            var deleteResult = await _viewModel.DeleteClientAsync(_viewModel.Selected.Id);
            _ = ShowMessage(ClientPageMessageType.DeletedClient, deleteResult.IsSuccess);
        }
    }

    private async void EditButton_Clicked(object sender, RoutedEventArgs e) {
        if (sender is Control button && button.Tag is ClientEditType tag && _viewModel.Selected != null) {
            var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "");
            var updateVM = new ClientUpdateViewModel(_viewModel.Selected);

            switch (tag) {
                case ClientEditType.Administrative:
                    dialog.Title = "Edit Client";
                    dialog.Content = new UpdateAdminsitrative(updateVM);
                    break;
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
    private async Task ToggleActiveAsync(bool isActive) {
        if (_viewModel.Selected?.Id is not int id) return;
        Debug.WriteLine($"Client Active: {isActive}");
        var result = isActive
            ? await _viewModel.MarkClientInactive(id)
            : await _viewModel.MarkClientActive(id);

        _ = ShowMessage(isActive ? ClientPageMessageType.MarkedInactive : ClientPageMessageType.MarkedActive, result.IsSuccess);
        if (!result.IsSuccess) return;

        _ = _viewModel.ReloadClientAsync();
    }

    private async Task ToggleTTWAsync(bool isTTW) {
        if (_viewModel.Selected?.Id is not int id) return;

        var result = isTTW
            ? await _viewModel.UnmarkClientTTW(id)
            : await _viewModel.MarkClientTTW(id);

        _ = ShowMessage(isTTW ? ClientPageMessageType.UnmarkedTTW : ClientPageMessageType.MarkedTTW, result.IsSuccess);
        if (!result.IsSuccess) return;

        _ = _viewModel.ReloadClientAsync();
    }

    private async Task UpdateCheckboxAsync(CheckboxTag tag, bool isChecked) {
        if (_viewModel.Selected == null) return;

        var updateVM = new ClientUpdateViewModel(_viewModel.Selected);
        updateVM.UpdateCheckbox(tag, isChecked);

        _ = UpdateClient(updateVM.UpdatedClient);
    }

    private async Task UpdateClient(ClientUpdate update) {
        var result = await _viewModel.UpdateClientAsync(update);
        _ = ShowMessage(ClientPageMessageType.UpdatedClient, result.IsSuccess);
    }

    private async Task ShowMessage(ClientPageMessageType type, bool success) {
        var infoBar = new InfoBar {
            Title = success ? "Success" : "Failed",
            Severity = success ? InfoBarSeverity.Success : InfoBarSeverity.Error,
            Message = Messages.MessageForType(type, success),
            IsOpen = true
        };

        InfoBarContainer.Children.Add(infoBar);

        await Task.Delay(success ? 2000 : 3000);
        InfoBarContainer.Children.Remove(infoBar);
    }
}
