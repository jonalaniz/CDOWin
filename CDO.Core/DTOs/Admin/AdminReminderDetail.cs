namespace CDO.Core.DTOs.Admin;

public record class AdminReminderDetail(
    int Id,
    DateTime Date,
    string Description,
    int ClientID,
    bool Complete,
    ReminderLog[] Logs,

    // Optional Fields
    string? ClientName
    ) {
    public string LocalDate => Date.ToString(format: "MM/dd/yyyy");
}