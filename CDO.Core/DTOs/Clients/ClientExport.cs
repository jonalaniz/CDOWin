using CDO.Core.DTOs.SAs;

namespace CDO.Core.DTOs.Clients;

public record class ClientExport(
    int Id,
    string FirstName,
    string LastName,
    string? DOB,
    string? DL,
    string? SSN,
    string City,
    string State,
    bool Active,
    bool TTW,
    string? CaseID,
    string? Address1,
    string? Address2,
    string? Zip,
    string? CounselorName,
    string? Phone,
    string? Phone2,
    string? Phone3,
    string? EmploymentGoal,
    string? Email,
    string? EmailDescription,
    string? Email2,
    string? EmailDescription2,
    SAExport[]? ServiceAuthorizations
);