using CDO.Core.Models;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Counselors.Inspectors;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Counselors;

public sealed partial class CounselorsPage : Page {
    public CounselorsViewModel ViewModel { get; }

    public CounselorsPage() {
        InitializeComponent();
        ViewModel = AppServices.CounselorsViewModel;
        DataContext = ViewModel;
        InspectorFrame.Navigate(typeof(CounselorInspector), ViewModel);
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is Counselor counselor) {
            _ = ViewModel.RefreshSelectedCounselor(counselor.id);
        }
    }
}
