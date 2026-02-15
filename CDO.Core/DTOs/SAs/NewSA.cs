namespace CDO.Core.DTOs.SAs;

public class NewSA {
    // SA Specific
    public required string ServiceAuthorizationNumber { get; init; }
    public string? Office { get; init; }
    public required string Description { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public double? UnitCost { get; init; }
    public string? UnitOfMeasurement { get; init; }

    // Client Specific
    public required int ClientID { get; init; }
    public required string ClientName { get; init; }
    public required string CaseID { get; init; }

    // Counselor Specific
    public int? CounselorID { get; init; }
    public required string CounselorName { get; init; }
    public string? SecretaryName { get; init; }
}