using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.Employers.Inspectors;

public sealed partial class EmployerInspector : Page {
    public EmployersViewModel? ViewModel { get; private set; }

    public EmployerInspector() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (EmployersViewModel)e.Parameter;
        DataContext = ViewModel;
    }
}
