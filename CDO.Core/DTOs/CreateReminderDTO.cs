namespace CDO.Core.DTOs;

public class CreateReminderDTO {
    // Required creation fields
    public required DateTime date { get; init; }
    public required string description { get; init; }

    // Optional fields
    public int clientID { get; init; }
    public string? clientName { get; init; }
    public bool complete { get; init; }
}