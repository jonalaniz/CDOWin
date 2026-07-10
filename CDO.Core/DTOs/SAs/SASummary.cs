namespace CDO.Core.DTOs.SAs;

public record class SASummary(
    // SA Specific
    int Id,
    bool Active,
    string ServiceAuthorizationNumber,
    DateTime StartDate,
    DateTime EndDate,
    double? UnitCost,
    string Description,

    // Client Specific
    int ClientId,
    string ClientName,
    string? CaseID,

    // Counselor Specific
    int? CounselorId,
    string CounselorName
) {
    // Computed Properties
    public string? FormattedStartDate => StartDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedEndDate => EndDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedCost => $"{UnitCost:C2}";
    public bool InActive => !Active;
}