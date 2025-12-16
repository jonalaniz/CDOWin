namespace CDO.Core.DTOs;

public class UpdateReminderDTO {
    public DateTime? date { get; set; }
    public string? description { get; set; }
    public int? clientID { get; set; }
    public string? clientName { get; set; }
    public bool? complete { get; set; }
}