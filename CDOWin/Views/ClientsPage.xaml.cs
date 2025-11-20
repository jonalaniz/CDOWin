using CDO.Core.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CDOWin.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ClientsPage : Page {
    public ClientsViewModel ViewModel { get; }
    public ClientsPage() {
        InitializeComponent();

        ViewModel = AppServices.ClientsViewModel;
        DataContext = ViewModel;
    }
}
