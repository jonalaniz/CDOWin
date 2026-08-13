namespace CDO.Core.DTOs.Admin;

public record class AdminReminderDetail(
    int Id,
    DateTime ActionDate,
    string Text,
    int ClientID,
    bool Completed,
    ReminderLog[] Logs,
    string? ClientName
    ) {
    public string LocalDate => ActionDate.ToString(format: "MM/dd/yyyy");
}