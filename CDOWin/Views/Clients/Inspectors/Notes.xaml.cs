using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;

namespace CDOWin.Views.Clients.Inspectors;

public sealed partial class Notes : Page {

    // =========================
    // ViewModel
    // =========================
    public ClientsViewModel ViewModel { get; private set; } = AppServices.ClientsViewModel!;

    // =========================
    // Constructor
    // =========================
    public Notes() {
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private void EditButton_Click(object sender, RoutedEventArgs e) {
        SaveButton.Visibility = Visibility.Visible;
        NewButton.Visibility = Visibility.Collapsed;
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e) {
        if (ViewModel == null || ViewModel.Selected == null) return;

        SaveButton.Visibility = Visibility.Collapsed;
        NewButton.Visibility = Visibility.Visible;

        var updateVM = new ClientUpdateViewModel(ViewModel.Selected);
        // updateVM.UpdatedClient.ClientNotes = NotesBox.Text.NormalizeString();

        var result = await ViewModel.UpdateClientAsync(updateVM.UpdatedClient);
        if (!result.IsSuccess)
            ErrorHandler.Handle(result, this.XamlRoot);
    }

    private async void NewButton_Click(object sender, RoutedEventArgs e) {
        if (ViewModel.Selected == null || sender is null) return;
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, "New Note");
        var createNoteVM = AppServices.CreateNoteViewModel(ViewModel.Selected.Id);
        var createNotePage = new CreateNote(createNoteVM);
        dialog.Content = createNotePage;
        dialog.IsPrimaryButtonEnabled = createNoteVM.CanSave;

        PropertyChangedEventHandler handler = (_, args) => {
            if (args.PropertyName == nameof(createNoteVM.CanSave))
                dialog.IsPrimaryButtonEnabled = createNoteVM.CanSave;
        };

        createNoteVM.PropertyChanged += handler;

        var result = await dialog.ShowAsync();
        createNoteVM.PropertyChanged -= handler;

        if (result != ContentDialogResult.Primary) return;

        var updateResult = await createNoteVM.CreateClientNoteAsync();
        if (!updateResult.IsSuccess) {
            ErrorHandler.Handle(updateResult, this.XamlRoot);
            return;
        }

        _ = ViewModel.ReloadClientAsync();
    }
}
