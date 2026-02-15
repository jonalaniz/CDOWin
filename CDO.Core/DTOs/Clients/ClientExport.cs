using CDO.Core.DTOs.SAs;

namespace CDO.Core.DTOs.Clients;

public class ClientExport() {
    // Non-optional fields
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }
    public required bool Active { get; init; }
    public required bool TTW { get; init; }

    // Nullable fields
    public string? Address1 { get; init; }
    public string? Address2 { get; init; }
    public string? Zip { get; init; }
    public string? CounselorName { get; init; }
    public string? Phone { get; init; }
    public string? Phone2 { get; init; }
    public string? Phone3 { get; init; }
    public string? EmploymentGoal { get; init; }
    public string? CaseID { get; init; }

    public SAExport[]? ServiceAuthorizations { get; init; }
}