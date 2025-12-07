using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateArrangements : Page {
    public ClientUpdateViewModel ViewModel;

    public UpdateArrangements(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = viewModel;
    }
}
