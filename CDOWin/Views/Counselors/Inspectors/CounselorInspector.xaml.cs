using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CDOWin.Views.Counselors.Inspectors;

public sealed partial class CounselorInspector : Page {
    public CounselorsViewModel? ViewModel { get; private set; }
    public CounselorInspector() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (CounselorsViewModel)e.Parameter;
        DataContext = ViewModel;
    }
}
