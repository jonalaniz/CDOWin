namespace CDO.Core.DTOs.Reminders;

public record class ReminderUpdate {
    public DateTime? ActionDate { get; set; }
    public string? Text { get; set; }
    public int? ClientID { get; set; }
    public string? ClientName { get; set; }
    public bool? Completed { get; set; }
}