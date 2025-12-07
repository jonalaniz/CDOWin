using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateContacts : Page {
    public ClientUpdateViewModel ViewModel { get; private set; }

    public UpdateContacts(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = viewModel.UpdatedClient;
    }
}
