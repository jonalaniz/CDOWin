namespace CDO.Core.DTOs.Admin;

public class UserSummary {
    // Non-optional fields
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required bool Admin { get; init; }
    
    // Nullable fields
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}