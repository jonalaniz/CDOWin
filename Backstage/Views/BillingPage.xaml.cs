using Backstage.Services;
using Backstage.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Backstage.Views;

public sealed partial class BillingPage : Page {

    // =========================
    // ViewModel
    // =========================
    public BillingViewModel ViewModel { get; } = AppServices.BillingViewModel;

    // =========================
    // Constructor
    // =========================
    public BillingPage() {
        InitializeComponent();
    }

    // =========================
    // Navigation
    // =========================
    protected override async void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        await ViewModel.LoadSASummariesAsync();
    }
}
