using CDOWin.Composers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace CDOWin.Views.Placements.Dialogs;

public sealed partial class ExportPlacement : Page {
    private readonly PlacementComposer _composer;
    public ExportPlacement(PlacementComposer composer) {
        _composer = composer;
        InitializeComponent();
    }

    private void DropGrid_DragOver(object sender, DragEventArgs e) {
        if (!e.DataView.Contains(StandardDataFormats.StorageItems)) return;
        e.AcceptedOperation = DataPackageOperation.Link;
    }

    private async void DropGrid_Drop(object sender, DragEventArgs e) {

        // Check if the dropped data contains files
        if (!e.DataView.Contains(StandardDataFormats.StorageItems)) return;

        // Get the file(s) from the DataPackage
        var items = await e.DataView.GetStorageItemsAsync();

        if (items[0] is not StorageFile file || file.FileType.ToLower() is not ".pdf") {
            DisplayError();
            return;
        }

        _composer.FilePath = file.Path;

        DropTextBlock.Text = "Ready to export";
    }

    private void DropGrid_DragLeave(object sender, DragEventArgs e) {

    }

    // Utilities

    private void DisplayError() {
        DropTextBlock.Text = "Not a PDF";
    }
}
