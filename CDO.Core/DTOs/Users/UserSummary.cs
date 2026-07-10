namespace CDO.Core.DTOs.Admin;

public record class UserSummary(
    string Id,
    string Username,
    bool Admin,
    bool Active,
    string? FirstName,
    string? LastName
) {
    public bool Inactive => !Active;
}