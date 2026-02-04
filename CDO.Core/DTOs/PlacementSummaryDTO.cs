namespace CDO.Core.DTOs;

public class PlacementSummaryDTO {
    // IDs: Optional, point to other entities in DB
    public required string Id { get; init; }
    public int? EmployerID { get; init; }
    public int? ClientID { get; init; }
    public int? CounselorID { get; init; }
    public int? InvoiceID { get; init; }

    // Names: Historical records of relationships above
    public string? ClientName { get; init; }
    public string? CounselorName { get; init; }
    public string? EmployerName { get; init; }
    public string? SaNumber { get; init; }
    public string? SupervisorName { get; init; }
    public string? SupervisorPhone { get; init; }
    public string? Website { get; init; }

    // Other historical fields
    public string? Position { get; init; }
    public string? Salary { get; init; }
    public DateTime? HireDate { get; init; }
    public bool? Active { get; init; }

    // Computed Properties
    public string? FormattedHireDate => HireDate?.ToString(format: "MM/dd/yyyy");

    public string? FormattedSalary => $"${Salary}";

    public string? FormattedSupervisor {
        get {
            var text = $"{SupervisorName}\n{SupervisorPhone}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
}
