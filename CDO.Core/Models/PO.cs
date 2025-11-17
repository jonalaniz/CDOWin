namespace CDO.Core.Models;

public record class PO(
    string id,
    int clientID,
    string description,
    string? office,
    int? employerID,
    double? unitCost,
    string? unitOfMeasurement,
    DateTime startDate,
    DateTime endDate
    ) {
    public DateTime? startDateLocal => startDate.ToLocalTime();
    public DateTime? endDateLocal => endDate.ToLocalTime();
}
