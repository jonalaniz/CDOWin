namespace CDO.Core.DTOs.SAs;

public record class NewSA(
    // SA Specific
    string ServiceAuthorizationNumber,
    string? Office,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    double? UnitCost,
    string? UnitOfMeasurement,

    // Client Specific
    int ClientID,
    string ClientName,
    string CaseID,

    // Counselor Specific
    int? CounselorID,
    string CounselorName,
    string? SecretaryName
);