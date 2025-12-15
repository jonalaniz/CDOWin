using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDOWin.Views;

public static class DialogFactory {
    public static ContentDialog UpdateDialog(XamlRoot root, string title) {
        ContentDialog dialog = new();
        dialog.XamlRoot = root;
        dialog.Style = dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.PrimaryButtonText = "Save";
        dialog.CloseButtonText = "Cancel";
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Title = title;

        return dialog;
    }
}
