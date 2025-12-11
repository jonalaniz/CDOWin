using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views;

public sealed partial class CounselorsPage : Page {
    public CounselorsViewModel ViewModel { get; }

    public CounselorsPage() {
        InitializeComponent();
        ViewModel = AppServices.CounselorsViewModel;
        DataContext = ViewModel;
    }
}
