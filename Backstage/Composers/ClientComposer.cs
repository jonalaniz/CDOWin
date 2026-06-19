using CDO.Core.DTOs.Admin;
using CDO.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Windows.Storage.Pickers.Provider;

namespace Backstage.Composers; 
class ClientComposer {
    private string _filePath = Path.GetTempPath() + "all_clients.csv";
    private StringBuilder csv = new();

    public void BuildCSV(List<AdminClientDetail> list) {
        csv.AppendLine("ACE ID, First Name, Last Name, DoB, Counserlor ID, Case ID, Active, TTW, Drivers License, SSN, Address, Address (cont.), City, State, Zip, Start Date, Created At, Last Updated, Disability");

        foreach (var client in list)
            csv.AppendLine(
                $"{client.Id}," +
                $"{client.FirstName}," +
                $"{client.LastName}," +
                $"{FormattedDate(client.Dob)}," +
                $"{client.CounselorID.ToString()}," +
                $"{client.CaseID ?? ""}," +
                $"{client.Active.ToString()}," +
                $"{client.Ttw.ToString()}," +
                $"{client.DriversLicense}," +
                $"{client.Ssn}," +
                $"{client.Address1}," +
                $"{client.Address2}," +
                $"{client.City}," +
                $"{client.State}," +
                $"{client.Zip}," +
                $"{FormattedDate(client.StartDate)}," +
                $"{FormattedDate(client.CreatedAt)}," +
                $"\"{client.Disability.Replace("\"", "\"\"")}\""
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

    private string FormattedDate(DateTime? date) {
        if (date is not DateTime unwrappedDate) return "";
        return unwrappedDate.ToString(format: "MM/dd/yyyy");
    }
}
