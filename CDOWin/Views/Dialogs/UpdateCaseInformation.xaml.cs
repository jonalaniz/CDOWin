using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateCaseInformation : Page {
    public ClientUpdateViewModel ViewModel { get; private set; }

    public UpdateCaseInformation(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = viewModel.UpdatedClient;
    }
}
