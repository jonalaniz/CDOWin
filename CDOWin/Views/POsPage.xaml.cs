using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views;

public sealed partial class POsPage : Page {
    public POsViewModel ViewModel { get; }

    public POsPage() {
        InitializeComponent();
        ViewModel = AppServices.POsViewModel;
        DataContext = ViewModel;
    }
}
