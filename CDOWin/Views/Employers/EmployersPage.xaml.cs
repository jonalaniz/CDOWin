using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Employers.Inspectors;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Employers;

public sealed partial class EmployersPage : Page {
    public EmployersViewModel ViewModel { get; }

    public EmployersPage() {
        InitializeComponent();
        ViewModel = AppServices.EmployersViewModel;
        DataContext = ViewModel;
        InspectorFrame.Navigate(typeof(EmployerInspector), ViewModel);
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Employer employer) {
            ViewModel.RefreshSelectedEmployer(employer.id);
        }
    }
}
