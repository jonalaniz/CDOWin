namespace CDO.Core.DTOs.SAs;

public record class InvoiceDetail(
    // SA Specific
    int Id,
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
        return new SAExport {
            SANumber = ServiceAuthorizationNumber,
            Description = Description,
            StartDate = StartDate.ToString(format: "MM/dd/yyyy"),
            EndDate = EndDate.ToString(format: "MM/dd/yyyy"),
            Office = Office
        };
    }
}

public class SAExport {
    public string? SANumber { get; init; }
    public string? Description { get; init; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Office { get; set; }
}