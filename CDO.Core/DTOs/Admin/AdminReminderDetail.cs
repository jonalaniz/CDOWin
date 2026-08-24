namespace CDO.Core.DTOs.Admin;

public record class AdminReminderDetail(
    int Id,
    DateTime Date,
    string Text,
    int ClientID,
    bool Completed,
    ReminderLog[] Logs,
    string? ClientName
    ) {
    public string LocalDate => Date.ToString(format: "MM/dd/yyyy");
}