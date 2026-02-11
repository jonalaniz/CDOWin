using CDO.Core.DTOs.Clients;

namespace CDO.Core.Models;

public record class Invoice(
    int Id,
    string ServiceAuthorizationNumber,
    string ClientName,
    string CaseID,
    string CounselorName,
    string Description,
    string? Office,
    int? CounselorID,
    int? ClientID,
    double? UnitCost,
    string? UnitOfMeasurement,
    ClientDetail? Client,
    DateTime StartDate,
    DateTime EndDate
    ) {
    public string? FormattedStartDate => StartDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedEndDate => EndDate.ToString(format: "MM/dd/yyyy");
    public string? FormattedCost => $"{UnitCost:C2}";
    public string? FormattedDateRange => $"Valid {FormattedStartDate} to {FormattedEndDate}";

    public static Invoice InjectClient(Invoice invoice, ClientDetail client) {
        return new Invoice(
            invoice.Id,
            invoice.ServiceAuthorizationNumber,
            invoice.ClientName,
            invoice.CaseID,
            invoice.CounselorName,
            invoice.Description,
            invoice.Office,
            invoice.CounselorID,
            invoice.ClientID,
            invoice.UnitCost,
            invoice.UnitOfMeasurement,
            client,
            invoice.StartDate,
            invoice.EndDate
        );
    }

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