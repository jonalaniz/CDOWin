using CDOWin.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views;

public sealed partial class ServiceAuthorizationsPage : Page {
    public ServiceAuthorizationsViewModel ViewModel { get; }

    public ServiceAuthorizationsPage() {
        InitializeComponent();
        ViewModel = AppServices.POsViewModel;
        DataContext = ViewModel;
    }
}
