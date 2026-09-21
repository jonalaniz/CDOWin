using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;
using CommunityToolkit.Mvvm.ComponentModel;
using Spire.Pdf;
using Spire.Pdf.Fields;
using Spire.Pdf.Widget;
using System;
using System.Diagnostics;
using System.IO;

namespace CDOWin.Composers;

public partial class PlacementComposer(PlacementDetail placement) : ObservableObject {
    private readonly PdfDocument Doc = new();
    private readonly PlacementDetail _placement = placement;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanExport))]
    public partial string? FilePath { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanExport => !String.IsNullOrEmpty(FilePath);

    public Result ComposePDF() {
        // Get the PDF path
        if (FilePath is not string path)
            return Result.Fail(new AppError(ErrorKind.Unknown, "PDF not found."));

        // Load the PDF
        Doc.LoadFromFile(path);

        // Grab the form
        if (Doc.Form is not PdfFormWidget formWidget)
            return Result.Fail(new AppError(ErrorKind.Unknown, "Unable to get Form Widget"));

        // Loop over the form items and fill in the form
        foreach (PdfField field in formWidget.FieldsWidget.List) {
            switch (field) {
                case PdfTextBoxFieldWidget textBox:
                    if (TextBoxString(textBox.Name) is string value && string.IsNullOrWhiteSpace(textBox.Text))
                        textBox.Text = value;
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

    string? TextBoxString(string nameString) {
        if (PTextField.Name(nameString) is not PTextFieldName fieldName) return null;

        return fieldName switch {
            // Placement Specific
            PTextFieldName.Position => _placement.Position,

            //HireDate => HireDate,
            PTextFieldName.Day1 => _placement.Day1,
            PTextFieldName.Day2 => _placement.Day2,
            PTextFieldName.Day3 => _placement.Day3,
            PTextFieldName.Day4 => _placement.Day4,
            PTextFieldName.Day5 => _placement.Day5,
            PTextFieldName.JobDuties => _placement.JobDuties,
            PTextFieldName.WorkEnvironment => _placement.WorkEnvironment,
            PTextFieldName.Accommodations => _placement.Accommodations,
            //Wages,
            PTextFieldName.Benefits => _placement.Benefits,

            // Client Specific
            PTextFieldName.ClientName => _placement.ClientName,
            PTextFieldName.SANumber => _placement.SaNumber,
            PTextFieldName.CaseID => _placement.CaseID,

            // Employer Specific
            PTextFieldName.EmployerName => _placement.EmployerName,
            PTextFieldName.EmployerPhone => _placement.EmployerPhone,
            PTextFieldName.SupervisorName => _placement.SupervisorName,
            PTextFieldName.SupervisorEmail => _placement.SupervisorEmail,
            PTextFieldName.SupervisorPhone => _placement.SupervisorPhone,
            PTextFieldName.Website => _placement.Website,
            PTextFieldName.Address => _placement.Address1 + _placement.Address2,
            PTextFieldName.City => _placement.City,
            PTextFieldName.State => _placement.State,
            PTextFieldName.Zip => _placement.Zip,
            _ => throw new NotImplementedException()
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