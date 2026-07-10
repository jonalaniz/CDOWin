namespace CDO.Core.DTOs.Admin;

public record class AdminSASummary(
    // SA
    int Id,
    bool Active,
    string ServiceAuthorizationNumber,
    DateTime StartDate,
    DateTime EndDate,
    double? UnitCost,
    string Description,

    // Client
    string ClientName,
    int ClientID,
    string? CaseID,

    // Counselor
    string CounselorName
    ) {
    // Computed Properties
    public string? FormattedDateRange => $"Valid {FormattedStartDate} to {FormattedEndDate}";
    public string? FormattedStartDate => StartDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedEndDate => EndDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedCost => $"{UnitCost:C2}";
    public bool InActive => !Active;
}