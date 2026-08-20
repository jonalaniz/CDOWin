using CDO.Core.DTOs.Clients;
using CDO.Core.DTOs.Clients.Notes;
using CDO.UI.Shared.Factories;
using CDOWin.Composers;
using CDOWin.ErrorHandling;
using CDOWin.Services;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Dialogs;
using CDOWin.Views.Shared.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Linq;

namespace CDOWin.Views.Clients.Inspectors;

public sealed partial class Notes : Page {

    // =========================
    // ViewModel
    // =========================
    private readonly ClientsViewModel _viewModel;

    // =========================
    // Constructor
    // =========================
    public Notes() {
        _viewModel = AppServices.ClientsViewModel;
        InitializeComponent();
    }

    // =========================
    // Click Handlers
    // =========================
    private void Export_Click(object sender, RoutedEventArgs e) {
        if (_viewModel.FilteredNotes.Count > 0) {
            ExportNotes(_viewModel.FilteredNotes.ToArray());
        }
    }

    private async void NewButton_Click(object sender, RoutedEventArgs e) {
        if (_viewModel.Selected == null || sender is null) return;
        var dialog = DialogFactory.NewObjectDialog(this.XamlRoot, "New Note");
        var createNoteVM = AppServices.CreateNoteViewModel(_viewModel.Selected.Id);
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

        _ = _viewModel.ReloadClientAsync();
    }

    private async void Note_Click(object sender, RoutedEventArgs e) {
        if (sender is Button button && button.Tag is int id)
            ShowEditPageFor(id);
    }

    private async void Edit_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item && item.Tag is int id)
            ShowEditPageFor(id);

    }

    private async void Delete_Click(object sender, RoutedEventArgs e) {
        if (sender is MenuFlyoutItem item && item.Tag is int id)
            ShowDeletePageFor(id);
    }

    // =========================
    // Utility Methods
    // =========================
    private async void ShowDeletePageFor(int id) {
        if (_viewModel.Selected is ClientDetail selected
            && selected.ClientNotes != null
            && selected.ClientNotes.FirstOrDefault(x => x.Id == id) is ClientNote note) {
            var dialog = DialogFactory.DeleteDialog(this.XamlRoot, "Delete Note");
            dialog.Content = new DeletePage();

            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary) return;

            var deleteResult = await _viewModel.DeleteNoteAsync(selected.Id, note.Id);
            if (!deleteResult.IsSuccess) {
                ErrorHandler.Handle(deleteResult, this.XamlRoot);
                return;
            }

            _ = _viewModel.ReloadClientAsync();
        }
    }

    private async void ShowEditPageFor(int id) {
        if (_viewModel.Selected is ClientDetail selected
            && selected.ClientNotes != null
            && selected.ClientNotes.FirstOrDefault(x => x.Id == id) is ClientNote note) {
            var updateVM = new NoteUpdateViewModel(note);
            var dialog = DialogFactory.UpdateDialog(this.XamlRoot, $"Edit Note #{note.Id}");
            dialog.Content = new UpdateNote(updateVM);

            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary) return;

            var updateResult = await _viewModel.UpdateNoteAsync(updateVM.Updated, selected.Id, note.Id);
            if (!updateResult.IsSuccess) {
                ErrorHandler.Handle(updateResult, this.XamlRoot);
                return;
            }

            _ = _viewModel.ReloadClientAsync();
        }
    }

    private async void ExportNotes(ClientNote[] notes) {
        var composer = new NotesComposer();
        var result = composer.ComposeNotesToFile(notes);
        if (!result.IsSuccess) ErrorHandler.Handle(result, this.XamlRoot);
    }
}
