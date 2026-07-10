namespace CDO.Core.DTOs.Admin;

public record class NewUser(
    string Username,
    string? FirstName,
    string? LastName,
    string? Password
);