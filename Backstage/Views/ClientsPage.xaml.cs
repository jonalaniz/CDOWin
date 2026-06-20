using Backstage.Services;
using Backstage.ViewModels;
using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace Backstage.Views;

public sealed partial class ClientsPage : Page {

    // =========================
    // ViewModel
    // =========================
    public ClientViewModel ViewModel { get; } = AppServices.ClientViewModel;

    public ClientsPage() {
        InitializeComponent();
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await ViewModel.RefreshAsync();
    }

    private async void ToggleInactive_Click(object sender, RoutedEventArgs e) {
        if (ViewModel.Selected is not AdminClientSummary summary) return;
        var result = summary.Active
            ? await ViewModel.MarkClientInactive(summary.Id)
            : await ViewModel.MarkClientActive(summary.Id);
        if (result.IsSuccess) ViewModel.ToggleActive(summary.Id);
        await ShowMessage(MessageType.MarkedTTW, result.IsSuccess);
    }

    private async void ToggleTTW_Click(object sender, RoutedEventArgs e) {
        if (ViewModel.Selected is not AdminClientSummary summary) return;
        var result = summary.Ttw
            ? await ViewModel.UnmarkClientTTW(summary.Id)
            : await ViewModel.MarkClientTTW(summary.Id);
        if (result.IsSuccess) ViewModel.ToggleTTW(summary.Id);
        await ShowMessage(MessageType.MarkedTTW, result.IsSuccess);
    }

    private async void ExportAllClients_Click(object sender, RoutedEventArgs e) {
        if (sender is not MenuFlyoutItem item) return;
        item.IsEnabled = false;

        var infoBar = new InfoBar {
            Title = "Exporting",
            Severity = InfoBarSeverity.Informational,
            Message = "This may take a while",
            IsOpen = true
        };

        InfoBarContainer.Children.Add(infoBar);

        var result = await ViewModel.ExportClients();
        item.IsEnabled = true;
        InfoBarContainer.Children.Remove(infoBar);

        await ShowMessage(MessageType.ExportedClients, result.IsSuccess);
    }

    private async Task ShowMessage(MessageType type, bool success) {
        var infoBar = new InfoBar {
            Title = success ? "Success" : "Failed",
            Severity = success ? InfoBarSeverity.Success : InfoBarSeverity.Error,
            Message = Messages.MessageForType(type, success),
            IsOpen = true
        };

        InfoBarContainer.Children.Add(infoBar);

        await Task.Delay(3000);
        InfoBarContainer.Children.Remove(infoBar);
    }
}
