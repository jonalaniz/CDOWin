namespace CDO.Core.DTOs.SAs;

public record class SADetail(
    // SA Specific
    int Id,
    bool Active,
    bool Billed,
    string ServiceAuthorizationNumber,
    string? Office,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    double? UnitCost,
    string? UnitOfMeasurement,

    // Client Specific
    int? ClientId,
    string ClientName,
    string CaseId,

    // Counselor Specific
    int? CounselorId,
    string CounselorName,
    string? SecretaryName
    ) {
    public string? FormattedStartDate => StartDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedEndDate => EndDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedCost => $"{UnitCost:C2}";
    public string? FormattedDateRange => $"Valid {FormattedStartDate} to {FormattedEndDate}";

    public SAExport AsExport() {
        return new(
            ServiceAuthorizationNumber,
            Description,
            StartDate.ToString(format: "MM/dd/yyyy"),
            EndDate.ToString(format: "MM/dd/yyyy"),
            Office
            );
    }
}

public record class SAExport(
    string? SANumber,
    string? Description,
    string? StartDate,
    string? EndDate,
    string? Office
);