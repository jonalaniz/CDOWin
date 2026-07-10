using CDO.Core.DTOs.Clients;
using CDO.Core.DTOs.SAs;

namespace CDO.Core.DTOs.Counselors;

public record class CounselorDetail(
    int Id,
    string Name,
    ClientSummary[] Clients,
    SADetail[] Sas,
    int? CaseLoadId,
    string? Phone,
    string? Email,
    string? Fax,
    string? Notes,
    string? SecretaryName,
    string? SecretaryEmail
);