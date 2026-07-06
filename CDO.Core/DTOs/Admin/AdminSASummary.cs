namespace CDO.Core.DTOs.Admin;

public class AdminSASummary {
    // SA Specific
    public int Id { get; init; }
    public bool Active { get; init; }
    public required string ServiceAuthorizationNumber { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public double? UnitCost { get; init; }
    public required string Description { get; init; }

    // Client Specific
    public required string ClientName { get; init; }
    public int ClientID { get; init; }
    public string? CaseID { get; init; }

    // Counselor Specific
    public required string CounselorName { get; init; }

    // Computed Properties
    public string? FormattedDateRange => $"Valid {FormattedStartDate} to {FormattedEndDate}";
    public string? FormattedStartDate => StartDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedEndDate => EndDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedCost => $"{UnitCost:C2}";
    public bool InActive => !Active;
}