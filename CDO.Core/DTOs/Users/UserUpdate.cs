namespace CDO.Core.DTOs.Users;

public record class UserUpdate {
    public bool? Admin { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
}