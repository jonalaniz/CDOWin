namespace CDO.Core.DTOs.Counselors;

public record class NewCounselor(
    string Name,
    int? CaseLoadID,
    string? Email,
    string? Phone,
    string? Fax,
    string? Notes,
    string? SecretaryName,
    string? SecretaryEmail
);