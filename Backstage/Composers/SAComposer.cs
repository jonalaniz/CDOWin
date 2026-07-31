using CDO.Core.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Backstage.Composers;

class SAComposer {
    private string _filePath = Path.GetTempPath() + "all_sa.csv";
    private StringBuilder csv = new();

    public void BuildCSV(List<AdminSASummary> list) {
        csv.AppendLine("Client Name, Case ID, Counselor Name, SA Number, Description, Start Date, End Date");

        foreach (var sa in list)
            csv.AppendLine(
                $"{sa.ClientName}," +
                $"{sa.CaseID}," +
                $"{sa.CounselorName}," +
                $"{sa.ServiceAuthorizationNumber}," +
                $"{sa.Description}," +
                $"{sa.FormattedStartDate}," +
                $"{sa.FormattedEndDate},"
                );

        File.WriteAllText(_filePath, csv.ToString(), Encoding.UTF8);

        try {
            Process.Start(new ProcessStartInfo {
                FileName = _filePath,
                UseShellExecute = true
            });
        } catch (Exception ex) {
            Debug.WriteLine("Could not open Excel: " + ex.Message);
        }
    }
}