namespace CDO.Core.DTOs.Reminders;

public class ReminderUpdate {
    public DateTime? Date { get; set; }
    public string? Description { get; set; }
    public int? ClientID { get; set; }
    public string? ClientName { get; set; }
    public bool? Complete { get; set; }
}