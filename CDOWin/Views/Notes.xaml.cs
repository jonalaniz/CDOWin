using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CDOWin.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Notes : Page {
    public ClientsViewModel ViewModel { get; private set; }

    public Notes() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (ClientsViewModel)e.Parameter;
        DataContext = ViewModel;
    }
}
