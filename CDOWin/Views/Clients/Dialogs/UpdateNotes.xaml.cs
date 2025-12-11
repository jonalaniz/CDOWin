using CDOWin.Controls;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.Dialogs;

public sealed partial class UpdateNotes : Page {
    public ClientUpdateViewModel ViewModel;

    public UpdateNotes(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
    }

    private void LabeledMultiLinePair_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        if (sender is LabeledMultiLinePair pair) {
            var notes = BuildNotes(pair.innerTextBox.Text);
            ViewModel.UpdatedClient.clientNotes = notes;
        }
    }

    private string BuildNotes(string note) {
        var date = DateTime.Now;
        var end = "++++++++++++++++++++++++";
        var newNote = $"{date.ToString()}\n\n{note}\n\n{end}\n\n";
        return newNote + ViewModel.OriginalClient.clientNotes;
    }
}
