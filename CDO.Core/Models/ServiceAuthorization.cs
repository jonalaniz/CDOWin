namespace CDO.Core.Models;

public record class ServiceAuthorization(
    string Id,
    int ClientID,
    string Description,
    string? Office,
    int? CounselorID,
    double? UnitCost,
    string? UnitOfMeasurement,
    Client? Client,
    DateTime StartDate,
    DateTime EndDate
    ) {
    public string? FormattedStartDate => StartDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedEndDate => EndDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedCost => $"{UnitCost:C2}";
    public string? FormattedDateRange => $"Valid {FormattedStartDate} to {FormattedEndDate}";
}