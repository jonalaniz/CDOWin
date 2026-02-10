using CDO.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CDOWin.Composers;

class RemindersComposer {
    private string _filePath = System.IO.Path.GetTempPath() + "export.csv";
    private StringBuilder csv = new();

    public void BuildCSV(List<Reminder> list) {
        csv.AppendLine("Date,ClientDetail,Description");

        foreach (var reminder in list)
            csv.AppendLine(
                $"{reminder.Date:yyyy-MM-dd}," +
                $"{reminder.ClientName}," +
                $"\"{reminder.Description?.Replace("\"", "\"\"")}\""
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
