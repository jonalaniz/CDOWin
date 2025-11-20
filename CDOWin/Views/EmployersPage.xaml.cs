using CDO.Core.Services;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CDOWin.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EmployersPage : Page {
    public EmployersViewModel ViewModel { get; }
    public EmployersPage() {
        InitializeComponent();

        ViewModel = AppServices.EmployersViewModel;
        DataContext = ViewModel;
    }
}
