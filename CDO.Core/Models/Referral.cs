using CDO.Core.DTOs;

namespace CDO.Core.Models;

public record class Referral(
    string id,
    int? placementNumber,
    string? employerID,
    int? clientID,
    int? counselorID,
    string? poNumber,
    string? supervisor,
    string? supervisorEmail,
    string? supervisorPhone,
    string? position,
    string? salary,
    float? daysOnJob,
    string? clientName,
    string? counselorName,
    bool? active,
    string? website,
    string? descriptionOfDuties,
    string? numbersOfHoursWorking,
    string? firstFiveDays1,
    string? firstFiveDays2,
    string? firstFiveDays3,
    string? firstFiveDays4,
    string? firstFiveDays5,
    string? descriptionOfWorkSchedule,
    string? hourlyOrMonthlyWages,
    DateTime? hireDate,
    DateTime? endDate,

    // Optional Parents
    EmployerDTO? employer
    ) {
    public string? formattedHireDate => hireDate?.ToString(format: "MM/dd/yyyy");
    public string? formattedEndDate => endDate?.ToString(format: "MM/dd/yyyy");

    public string? formattedSalary => $"${salary}";

    public string? formattedSupervisor {
        get {
            var text = $"{supervisor}\n{supervisorPhone}\n{supervisorEmail}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
}