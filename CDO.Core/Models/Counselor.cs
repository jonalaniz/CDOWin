namespace CDO.Core.Models;

public record class Counselor(
    int Id,
    int? CaseLoadId,
    string Name,
    string? Email,
    string? Phone,
    string? Fax,
    string? Notes,
    string? SecretaryName,
    string? SecretaryEmail
    );
