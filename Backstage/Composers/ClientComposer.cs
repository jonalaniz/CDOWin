using CDO.Core.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Backstage.Composers;

class ClientComposer {
    private string _filePath = Path.GetTempPath() + "all_clients.csv";
    private StringBuilder csv = new();

    public void BuildCSV(List<AdminClientDetail> list) {
        csv.AppendLine("ACE ID, First Name, Last Name, DoB, Counserlor ID, Case ID, Active, TTW, " +
            "Drivers License, SSN, Address, Address (cont.), City, State, Zip, Start Date, Created At, " +
            "Last Updated, Disability, Tier, Benefits, Conditions, Employment Goal, Education, " +
            "Transportation, Criminal Charge, Phone Description, Phone, Phone 2 Description, " +
            "Phone 2, Phone 3 Description, Phone 3, Email Description, Email, Email 2 Description, Email 2, " +
            "Resume Required, Resume Completed, Video Interview Required, Video Interview Completed, " +
            "Releases, Orienttion, Data Sheet, Elevator Speech");

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
                $"\"{FormatText(client.Disability)}\"" +
                $"\"{FormatText(client.Status)}\"" +
                $"\"{FormatText(client.Benefits)}\"" +
                $"\"{FormatText(client.Conditions)}\"" +
                $"\"{FormatText(client.EmploymentGoal)}\"" +
                $"\"{FormatText(client.Education)}\"" +
                $"\"{FormatText(client.Transportation)}\"" +
                $"\"{FormatText(client.CriminalCharge)}\"" +
                $"\"{FormatText(client.Phone1Identity)}\"" +
                $"\"{FormatText(client.Phone1)}\"" +
                $"\"{FormatText(client.Phone2Identity)}\"" +
                $"\"{FormatText(client.Phone2)}\"" +
                $"\"{FormatText(client.Phone3Identity)}\"" +
                $"\"{FormatText(client.Phone3)}\"" +
                $"\"{FormatText(client.EmailIdentity)}\"" +
                $"\"{FormatText(client.Email)}\"" +
                $"\"{FormatText(client.Email2Identity)}\"" +
                $"\"{FormatText(client.Email2)}\"" +
                $"{client.ResumeRequired.ToString()}," +
                $"{client.ResumeCompleted.ToString()}," +
                $"{client.VideoInterviewRequired.ToString()}," +
                $"{client.VideoInterviewCompleted.ToString()}," +
                $"{client.ReleasesCompleted.ToString()}," +
                $"{client.OrientationCompleted.ToString()}," +
                $"{client.DataSheetCompleted.ToString()}," +
                $"{client.ElevatorSpeechCompleted.ToString()},"
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

    private string? FormatText(string? text) {
        if (text == null) return string.Empty;
        return text.Replace("\"", "\"\"").Replace("\r\n\r\n", "\r\n");
    }
}
