namespace CDO.Core.DTOs.Placements;

public record class PlacementSummary(
    // ID
    int Id,

    // Placement Specific
    bool Active,
    bool Billed,
    string? Position,
    DateTime? HireDate,
    string? Wages,

    // SA/SADetail Specific
    int? SaID,
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
    string? SupervisorName,
    string? SupervisorPhone,
    string? Website
) {
    // Computed Properties
    public string FormattedSANumber => $"Assocaited SA: {SaNumber}";
    public string? FormattedHireDate => HireDate?.ToString(format: "MM/dd/yyyy");
    public string? FormattedSupervisor {
        get {
            var text = $"{SupervisorName}\n{SupervisorPhone}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
    public bool InActive => !Active;
}
