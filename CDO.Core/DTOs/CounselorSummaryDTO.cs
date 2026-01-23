namespace CDO.Core.DTOs;

public class CounselorSummaryDTO {
    // Non-optional fields
    public int Id { get; init; }
    public required string Name { get; init; }

    // Nullable fields
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? SecretaryName { get; init; }
}
