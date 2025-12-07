using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateEmploymentProfile : Page {
    public ClientUpdateViewModel ViewModel { get; private set; }
    public UpdateEmploymentProfile(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = viewModel.UpdatedClient;
    }
}
