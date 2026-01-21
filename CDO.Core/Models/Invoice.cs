namespace CDO.Core.Models;

public record class Invoice(
    int Id,
    string ServiceAuthorizationNumber,
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

    public static Invoice InjectClient(Invoice invoice, Client client) {
        return new Invoice(
            invoice.Id,
            invoice.ServiceAuthorizationNumber,
            invoice.Description,
            invoice.Office,
            invoice.CounselorID,
            invoice.UnitCost,
            invoice.UnitOfMeasurement,
            client,
            invoice.StartDate,
            invoice.EndDate
        );
    }
}