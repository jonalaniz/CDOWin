namespace CDO.Core.DTOs.Placements;

public class PlacementSummary {
    // ID
    public required int Id { get; init; }

    // Placement Specific
    public bool Active { get; init; }
    public string? Position { get; init; }
    public DateTime? HireDate { get; init; }
    public string? Wages { get; init; }

    // SA/InvoiceDetail Specific
    public int? InvoiceID { get; init; }
    public string? SaNumber { get; init; }

    // ClientDetail Specific
    public int? ClientID { get; init; }
    public string? ClientName { get; init; }

    // Counselor Specific
    public int? CounselorID { get; init; }
    public string? CounselorName { get; init; }

    // Employer Specific
    public int? EmployerID { get; init; }
    public string? EmployerName { get; init; }
    public string? SupervisorName { get; init; }
    public string? SupervisorPhone { get; init; }
    public string? Website { get; init; }

    // Computed Properties
    public string? FormattedHireDate => HireDate?.ToString(format: "MM/dd/yyyy");
    public string? FormattedSupervisor {
        get {
            var text = $"{SupervisorName}\n{SupervisorPhone}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
}
