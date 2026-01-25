namespace CDO.Core.DTOs;

public class CreateCounselorDTO {
    // Required creation fields
    public required string name { get; init; }

    // Optional fields
    public int? CaseLoadId { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Fax { get; init; }
    public string? Notes { get; init; }
    public string? SecretaryName { get; init; }
    public string? SecretaryEmail { get; init; }
}