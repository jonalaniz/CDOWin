namespace CDO.Core.DTOs.SAs;

public class NewSA {
    // Required creation fields
    public required string ServiceAuthorizationNumber { get; init; }
    public required int ClientID { get; init; }
    public required string ClientName { get; init; }
    public required string CounselorName { get; init; }
    public required string CaseID { get; init; }
    public required string Description { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }

    // Optional fields
    public string? Office { get; set; }
    public int? CounselorID { get; set; }
    public double? UnitCost { get; set; }
    public string? UnitOfMeasurement { get; set; }
}