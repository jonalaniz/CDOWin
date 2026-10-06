using Backstage.Services;
using Backstage.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Backstage.Views;

public sealed partial class UsersPage : Page {

    // =========================
    // ViewModel
    // =========================
    private readonly UserViewModel _viewModel = AppServices.UserViewModel;

    // =========================
    // Constructor
    // =========================
    public UsersPage() => InitializeComponent();

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await _viewModel.RefreshAsync();
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) { }

    private void Refresh_ItemCLick(object sender, RoutedEventArgs e) => _ = _viewModel.RefreshAsync(force: true);
}
