namespace CDO.Core.DTOs.SAs;

public class InvoiceSummary {
    // SA Specific
    public int Id { get; init; }
    public required string ServiceAuthorizationNumber { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public double? UnitCost { get; init; }
    public required string Description { get; init; }

    // Client Specific
    public int ClientId { get; init; }
    public required string ClientName { get; init; }
    public string? CaseID { get; init; }

    // Counselor Specific
    public int? CounselorId { get; init; }
    public required string CounselorName { get; init; }

    // Computed Properties
    public string? FormattedStartDate => StartDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedEndDate => EndDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedCost => $"{UnitCost:C2}";
}