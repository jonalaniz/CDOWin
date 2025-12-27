namespace CDO.Core.DTOs;

public class CreateReminderDTO {
    // Required creation fields
    public required DateTime date { get; set; }
    public required string description { get; set; }

    // Optional fields
    public int clientID { get; set; }
    public string? clientName { get; set; }
    public bool complete { get; set; }
}