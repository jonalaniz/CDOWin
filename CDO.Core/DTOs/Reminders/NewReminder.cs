namespace CDO.Core.DTOs.Reminders;

public class NewReminder {
    // Required creation fields
    public required DateTime Date { get; set; }
    public required string Description { get; set; }

    // Optional fields
    public int ClientID { get; set; }
    public string? ClientName { get; set; }
    public bool Complete { get; set; }
}