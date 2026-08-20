namespace CDO.Core.DTOs.Admin;

public record class ReminderLog(
    UserSummary? User,
    DateTime CreatedAt,
    DateTime ActionDate,
    string Text,
    bool Completed
    ) {
    public string LocalDate => CreatedAt.ToString(format: "MM/dd/yy");
    public string LocalActionDate => ActionDate.ToString(format: "MM/dd/yy");
}