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

    private void SA_Reminder_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (sender is not Button button || button.Tag is not int id) return;

    }
}
