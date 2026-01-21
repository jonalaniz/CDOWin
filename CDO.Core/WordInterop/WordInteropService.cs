using CDO.Core.Models;
using System.Diagnostics;
using Word = Microsoft.Office.Interop.Word;

namespace CDO.Core.WordInterop;

public sealed class WordInteropService {
    public void ExportServiceAuthorization(string templatePath, Invoice sa) {
        Debug.WriteLine(templatePath);

        var app = new Word.Application();

        var doc = app.Documents.Add(templatePath);

        try {
            foreach (Word.FormField field in doc.FormFields) {
                switch (field.Name) {
                    case "ID":
                        field.Result = sa.ServiceAuthorizationNumber;
                        break;
                    case "DRSOffice":
                        field.Result = sa.Office ?? "";
                        break;
                    case "ClientName":
                        field.Result = $"{sa.Client?.FirstName} {sa.Client?.LastName}";
                        break;
                    case "CaseID":
                        field.Result = sa.Client?.CaseID ?? "";
                        break;
                    case "StartDate":
                        field.Result = sa.StartDate.ToString(format: "MM/dd/yyyy") ?? "";
                        break;
                    case "EndDate":
                        field.Result = sa.EndDate.ToString(format: "MM/dd/yyyy") ?? "";
                        break;
                    case "SecretaryName":
                        field.Result = sa.Client?.CounselorReference?.SecretaryName ?? "";
                        break;
                    case "CounselorName":
                        field.Result = sa.Client?.CounselorReference?.Name ?? "";
                        break;
                    case "UM":
                        field.Result = sa.UnitOfMeasurement ?? "";
                        break;
                    case "SADescription":
                        field.Result = sa.Description;
                        break;
                    case "UnitCost":
                        field.Result = sa.UnitCost?.ToString("C2") ?? "";
                        break;
                    case "TotalLineCost":
                        field.Result = sa.UnitCost?.ToString("C2") ?? "";
                        break;
                    case "GrandTotal":
                        field.Result = sa.UnitCost?.ToString("C2") ?? "";
                        break;
                }
            }

            // 3️⃣ NOW save the document wherever you want
            string outputPath = Path.Combine(
                Path.GetTempPath(),
                $"ServiceAuthorization_{Guid.NewGuid()}.docx"
            );

            doc.SaveAs2(outputPath, Word.WdSaveFormat.wdFormatXMLDocument);

            app.Visible = true;
            doc.Activate();
        } catch (Exception ex) {
            Debug.WriteLine(ex.ToString());
        }
    }
}
