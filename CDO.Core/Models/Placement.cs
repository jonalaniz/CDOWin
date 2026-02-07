using CDO.Core.DTOs;

namespace CDO.Core.Models;

public record class Placement(
    string Id,
    int? PlacementNumber,
    int? EmployerID,
    int? ClientID,
    int? CounselorID,
    int? InvoiceID,
    string? SaNumber,
    string? SupervisorName,
    string? SupervisorEmail,
    string? SupervisorPhone,
    string? Position,
    string? Salary,
    float? DaysOnJob,
    string? ClientName,
    string? CounselorName,
    string? EmployerName,
    bool? Active,
    string? Website,
    string? JobDuties,
    string? HoursWorking,
    string? Day1,
    string? Day2,
    string? Day3,
    string? Day4,
    string? Day5,
    string? WorkSchedule,
    string? Wages,
    DateTime? HireDate,
    DateTime? EndDate,

    // Optional Parents
    EmployerDTO? Employer
    ) {
    public string? FormattedHireDate => HireDate?.ToString(format: "MM/dd/yyyy");

    public string? FormattedEndDate => EndDate?.ToString(format: "MM/dd/yyyy");

    public string? FormattedSalary => $"${Salary}";

    public string? FormattedSupervisor {
        get {
            var text = $"{SupervisorName}\n{SupervisorPhone}\n{SupervisorEmail}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
}