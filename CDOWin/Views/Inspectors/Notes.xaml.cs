using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.Inspectors;

public sealed partial class Notes : Page {
    public ClientsViewModel? ViewModel { get; private set; }

    public Notes() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (ClientsViewModel)e.Parameter;
        DataContext = ViewModel;
    }
}
