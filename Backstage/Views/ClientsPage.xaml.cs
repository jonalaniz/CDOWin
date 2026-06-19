using Backstage.Services;
using Backstage.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

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
}
