using CDO.Core.DTOs;
using CDOWin.ViewModels;
using CDOWin.Views.Counselors.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        // TODO: CHECK THAT VIWMODEL.SELECTED ISN'T NULL
        // THIS ENSURES NOTHING HAPPENS IF THEY CLICK AND NOTHING IS SELECTED
        var updateVM = new CounselorUpdateViewModel(ViewModel.Selected);
        var dialog = DialogFactory.UpdateDialog(this.XamlRoot, "Edit Counselor");
        dialog.Content = new UpdateCounselor(updateVM);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary) {
            updateCounselor(updateVM.Updated);
        }
    }

    private void updateCounselor(UpdateCounselorDTO update) {
        Debug.WriteLine("UPDATING");
        // Here we need ot implement ViewModel.UpdateCounselor
    }
}
