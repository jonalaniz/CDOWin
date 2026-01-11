using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views;

public static class DialogFactory {
    public static ContentDialog UpdateDialog(XamlRoot root, string title) {
        ContentDialog dialog = Dialog(root, title);
        dialog.PrimaryButtonText = "Save";
        dialog.Title = title;
        return dialog;
    }

    public static ContentDialog NewObjectDialog(XamlRoot root, string title) {
        ContentDialog dialog = Dialog(root, title);
        dialog.PrimaryButtonText = "Create";
        dialog.Title = title;
        return dialog;
    }

    public static ContentDialog DeleteDialog(XamlRoot root, string title) {
        ContentDialog dialog = Dialog(root, title);
        dialog.PrimaryButtonText = "Delete";
        dialog.Title = title;
        dialog.DefaultButton = ContentDialogButton.None;
        return dialog;
    }

    public static ContentDialog ErrorDialog(XamlRoot root, string error, string message) {
        ContentDialog dialog = Dialog(root, error);
        dialog.Content = message;
        dialog.CloseButtonText = "OK";
        return dialog;
        
    }

    private static ContentDialog Dialog(XamlRoot root, string title) {
        ContentDialog dialog = new();
        dialog.XamlRoot = root;
        dialog.Style = dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.CloseButtonText = "Cancel";
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Title = title;
        return dialog;
    }
}
