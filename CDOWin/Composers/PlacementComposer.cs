using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;
using CDO.Core.WordInterop;
using Spire.Pdf;
using Spire.Pdf.Fields;
using Spire.Pdf.Widget;
using System;
using System.Diagnostics;
using System.IO;

namespace CDOWin.Composers;

public sealed class PlacementComposer {
    private readonly PdfDocument Doc = new();
    private readonly ITemplateProvider _templateProvider = new TemplateProvider();

    public Result ComposePDF(PlacementDetail placement) {
        // Get the PDF path
        if (_templateProvider.GetTemplate("vr1845b-twc.pdf") is not string path)
            return Result.Fail(new AppError(ErrorKind.Unknown, "PDF Template not found."));

        // Load the PDF
        Doc.LoadFromFile(path);

        // Grab the form
        if (Doc.Form is not PdfFormWidget formWidget)
            return Result.Fail(new AppError(ErrorKind.Unknown, "Unable to get Form Widget"));

        // Loop over the form items and fill in the form
        foreach (PdfField field in formWidget.FieldsWidget.List) {
            switch (field) {
                case PdfTextBoxFieldWidget textBox:
                    textBox.Text = TextBoxString(textBox.Name);
                    break;
                case PdfRadioButtonListFieldWidget radioButton:
                    Debug.WriteLine($"Radio Button: {radioButton.Name}");
                    break;
                case PdfCheckBoxWidgetFieldWidget checkbox:
                    Debug.WriteLine($"Check Box: {checkbox.Name}");
                    break;
                default:
                    break;
            }
        }

        // Get the temp path
        var tempPath = Path.Combine(Path.GetTempPath(), "Placement.pdf");

        // Save and open the PDF
        try {
            Doc.SaveToFile(tempPath);
            var startInfo = new ProcessStartInfo(tempPath) { UseShellExecute = true };
            Process.Start(startInfo);
        } catch (Exception ex) {
            Debug.WriteLine(ex.ToString());
            return Result.Fail(new AppError(ErrorKind.Unknown, "Failed to export Placement", Exception: ex));
        }

        return Result.Success();
    }

    static string TextBoxString(string fieldName) {
        return fieldName switch {
            _ => ""
        };
    }
}

/*
if (field is PdfRadioButtonListFieldWidget) {
    PdfRadioButtonListFieldWidget radioBtnField = field as PdfRadioButtonListFieldWidget;
    switch (radioBtnField.Name) {
        case "country":
            radioBtnField.SelectedIndex = 1;
            break;
    }
}

if (field is PdfCheckBoxWidgetFieldWidget) {
    PdfCheckBoxWidgetFieldWidget checkBoxField = field as PdfCheckBoxWidgetFieldWidget;
    switch (checkBoxField.Name) {
        case "agreement_of_terms":
            checkBoxField.Checked = true;
            break;
    }
}
*/