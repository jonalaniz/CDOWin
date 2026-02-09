namespace CDO.Core.DTOs.Placements;

public record class PlacementDetail(
    // ID
    int Id,

    // Placement Specific
    bool? Active,
    int? PlacementNumber,
    string? Position,
    DateTime? HireDate,
    DateTime? EndDate,
    float? DaysOnJob,
    string? Day1,
    string? Day2,
    string? Day3,
    string? Day4,
    string? Day5,
    string? JobDuties,
    string? HoursWorking,
    string? WorkSchedule,
    string? Wages,
    string? Benefits,

    // SA/Invoice Specific
    int? InvoiceID,
    string? SaNumber,

    // Client Specific
    int? ClientID,
    string? ClientName,

    // Counselor Specific
    int? CounselorID,
    string? CounselorName,

    // Employer Specific
    int? EmployerID,
    string? EmployerName,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? Zip,
    string? SupervisorName,
    string? SupervisorEmail,
    string? SupervisorPhone,
    string? Website
    ) {
    public string? FormattedHireDate => HireDate?.ToString(format: "MM/dd/yyyy");
    public string? FormattedEndDate => EndDate?.ToString(format: "MM/dd/yyyy");

    public string? FormattedSupervisor {
        get {
            var text = $"{SupervisorName}\n{SupervisorPhone}\n{SupervisorEmail}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
}