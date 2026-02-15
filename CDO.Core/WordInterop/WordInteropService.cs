using CDO.Core.DTOs.SAs;
using System.Diagnostics;
using Word = Microsoft.Office.Interop.Word;

namespace CDO.Core.WordInterop;

public sealed class WordInteropService {
    public void ExportServiceAuthorization(string templatePath, InvoiceDetail invoice) {
        Debug.WriteLine(templatePath);

        var app = new Word.Application();

        var doc = app.Documents.Add(templatePath);

        try {
            foreach (Word.FormField field in doc.FormFields) {
                switch (field.Name) {
                    case "ID":
                        field.Result = invoice.ServiceAuthorizationNumber;
                        break;
                    case "DRSOffice":
                        field.Result = invoice.Office ?? "";
                        break;
                    case "ClientName":
                        field.Result = invoice.ClientName;
                        break;
                    case "CaseID":
                        field.Result = invoice.CaseId;
                        break;
                    case "StartDate":
                        field.Result = invoice.StartDate.ToString(format: "MM/dd/yyyy") ?? "";
                        break;
                    case "EndDate":
                        field.Result = invoice.EndDate.ToString(format: "MM/dd/yyyy") ?? "";
                        break;
                    case "SecretaryName":
                        field.Result = invoice.SecretaryName ?? "";
                        break;
                    case "CounselorName":
                        field.Result = invoice.CounselorName;
                        break;
                    case "UM":
                        field.Result = invoice.UnitOfMeasurement ?? "";
                        break;
                    case "SADescription":
                        field.Result = invoice.Description;
                        break;
                    case "UnitCost":
                        field.Result = invoice.UnitCost?.ToString("C2") ?? "";
                        break;
                    case "TotalLineCost":
                        field.Result = invoice.UnitCost?.ToString("C2") ?? "";
                        break;
                    case "GrandTotal":
                        field.Result = invoice.UnitCost?.ToString("C2") ?? "";
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
            app.Activate();
            doc.Activate();
        } catch (Exception ex) {
            Debug.WriteLine(ex.ToString());
        }
    }
}
