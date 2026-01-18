using CDO.Core.ErrorHandling;
using CDOWin.Views;
using Microsoft.UI.Xaml;
using System;

namespace CDOWin.ErrorHandling;

public sealed class ErrorHandler {
    static public async void Handle(Result result, XamlRoot xamlRoot) {
        if (result.Error is not AppError error) return;

        var message = error.Exception == null
            ? "Unknown Error"
            : $"{error.Exception.Source}: {error.Exception.StackTrace}";
        var dialog = DialogFactory.ErrorDialog(xamlRoot, error.Kind.ToString(), message);
        await dialog.ShowAsync();
    }
}
