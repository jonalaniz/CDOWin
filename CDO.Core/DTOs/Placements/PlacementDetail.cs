namespace CDO.Core.DTOs.Placements;

public record class PlacementDetail(
    // ID
    int Id,

    // Placement Specific
    bool Active,
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
    string? WorkEnvironment,
    string? Accommodations,
    string? HoursWorking,
    string? WorkSchedule,
    string? Wages,
    string? Benefits,

    // SA/InvoiceDetail Specific
    int? InvoiceID,
    string? SaNumber,

    // ClientDetail Specific
    int? ClientID,
    string? ClientName,

    // Counselor Specific
    int? CounselorID,
    string? CounselorName,

    // Employer Specific
    int? EmployerID,
    string? EmployerName,
    string? EmployerPhone,
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

    public string FormattedAddress {
        get {
            if (Address1 == null && Address2 == null)
                return "No address on file.";
            else if (Address2 == null) {
                return $"{Address1}\n{FormattedCityStateZip}";
            } else {
                return $"{Address1} {Address2}\n{FormattedCityStateZip}";
            }
        }
    }

    public string FormattedCityStateZip {
        get {
            if (Zip != null)
                return $"{City}, {State} {Zip}";
            else
                return $"{City}, {State}";
        }
    }

    public string? FormattedSupervisor {
        get {
            var text = $"{SupervisorName}\n{SupervisorPhone}\n{SupervisorEmail}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
}