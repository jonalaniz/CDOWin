using CDOWin.Controls;
using CDOWin.Extensions;
using CDOWin.ViewModels;
using CDOWin.Views.Clients.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace CDOWin.Views.Counselors.Dialogs;

public sealed partial class UpdateCounselor : Page {
    public CounselorUpdateViewModel ViewModel;
    public UpdateCounselor(CounselorUpdateViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel.Original;
        InitializeComponent();
    }

    private void LabeledTextBox_TextChangedForwarded(object sender, TextChangedEventArgs e) {
        string? originalValue = null;
        string? updatedValue = null;
        UpdateField? field = null;

        if (sender is LabeledMultiLinePair multiLinePair) {
            originalValue = multiLinePair.Value.NormalizeString();
            updatedValue = multiLinePair.innerTextBox.Text.NormalizeString();
            field = (UpdateField)multiLinePair.Tag;
        } else if (sender is LabeledTextBox multiLineTextBox) {
            originalValue = multiLineTextBox.Value.NormalizeString();
            updatedValue = multiLineTextBox.innerTextBox.Text.NormalizeString();
            field = (UpdateField)multiLineTextBox.Tag;
        }

        if (field == null || originalValue == updatedValue || updatedValue == null)
            return;

        UpdateModel(updatedValue, field.Value);
    }

    private void UpdateModel(string value, UpdateField field) {
        switch (field) {
            case UpdateField.Name:
                ViewModel.Updated.name = value;
                break;
            case UpdateField.Email:
                ViewModel.Updated.email = value;
                break;
            case UpdateField.Phone:
                ViewModel.Updated.phone = value;
                break;
            case UpdateField.Fax:
                ViewModel.Updated.fax = value;
                break;
            case UpdateField.Notes:
                ViewModel.Updated.notes = value;
                break;
            case UpdateField.Secretary:
                ViewModel.Updated.secretaryName = value;
                break;
            case UpdateField.SecretaryEmail:
                ViewModel.Updated.secretaryEmail = value;
                break;
        }
    }
}
