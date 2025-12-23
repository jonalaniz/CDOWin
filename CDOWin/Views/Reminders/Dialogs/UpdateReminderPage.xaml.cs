using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.Reminders.Dialogs;

public sealed partial class UpdateReminderPage : Page {
    public ReminderUpdateViewModel ViewModel;

    public UpdateReminderPage(ReminderUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.Original;
        InitializeComponent();
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {

    }
}
