namespace CDO.Core.DTOs.Users;

public class UserUpdate {
    public bool? Admin { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
}