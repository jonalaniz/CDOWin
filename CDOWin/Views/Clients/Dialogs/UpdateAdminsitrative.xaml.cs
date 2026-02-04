using CDOWin.Extensions;
using CDOWin.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Threading.Tasks;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateAdminsitrative : Page {

    // =========================
    // Dependencies
    // =========================
    public ClientUpdateViewModel ViewModel { get; private set; }

    // =========================
    // Constructor
    // =========================
    public UpdateAdminsitrative(ClientUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
    }

    // =========================
    // Click Hanlders
    // =========================
    private async void DocumentsButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        if (sender is Button button) {
            button.IsEnabled = false;

            var picker = new FolderPicker(button.XamlRoot.ContentIslandEnvironment.AppWindowId);
            picker.CommitButtonText = "Choose Folder";
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.ViewMode = PickerViewMode.List;

            var folder = await picker.PickSingleFolderAsync();
            if (folder != null) SetDocumentsFolder(folder.Path);

            button.IsEnabled = true;
        }
    }

    // =========================
    // Property Change Methods
    // =========================
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox || textbox.Tag is not AdministrativeField field) 
            return;

        var text = textbox.Text.NormalizeString();
        if (string.IsNullOrWhiteSpace(text)) return;
        UpdateValue(text, field);
    }

    // =========================
    // Utility Methods
    // =========================
    private void SetDocumentsFolder(string folder) {
        ViewModel.FolderPath = folder;
        UpdateValue(folder, AdministrativeField.DocumentsFolder);
    }

    private void UpdateValue(string value, AdministrativeField type) {
        switch(type) {
            case AdministrativeField.FirstName:
                ViewModel.UpdatedClient.FirstName = value;
                break;
            case AdministrativeField.LastName:
                ViewModel.UpdatedClient.LastName = value;
                break;
            case AdministrativeField.DocumentsFolder:
                ViewModel.UpdatedClient.DocumentFolder = value;
                break;
        }
    }
}
