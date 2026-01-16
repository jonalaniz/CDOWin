namespace CDO.Core.Models;

public record class ServiceAuthorization(
    string Id,
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

    public static ServiceAuthorization InjectClient(ServiceAuthorization sa, Client client) {
        return new ServiceAuthorization(
            sa.Id,
            sa.Description,
            sa.Office,
            sa.CounselorID,
            sa.UnitCost,
            sa.UnitOfMeasurement,
            client,
            sa.StartDate,
            sa.EndDate
        );
    }
}