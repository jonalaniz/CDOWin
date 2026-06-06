using Backstage.Services;
using Backstage.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Backstage.Views {
    public sealed partial class UsersPage : Page {

        // =========================
        // ViewModel
        // =========================
        public UserViewModel ViewModel { get; } = AppServices.UserViewModel;

        // =========================
        // Constructor
        // =========================
        public UsersPage() {
            InitializeComponent();
        }

        // =========================
        // Navigation
        // =========================
        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            await ViewModel.LoadUserSummariesAsync();
        }


    }
}
