namespace CDO.Core.DTOs.Admin;

public class NewUser {
    // Required creation fields
    public required string Username { get; init; }

    // Nullable fields
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Password { get; init; }
}