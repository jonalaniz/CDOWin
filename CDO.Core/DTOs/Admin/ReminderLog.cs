namespace CDO.Core.DTOs.Admin;

public record class ReminderLog(
    UserSummary? User,
    DateTime Date,
    DateTime ActionDate,
    string Text,
    bool Completed
    ) {
    public string LocalDate => Date.ToString(format: "MM/dd/yy");
    public string LocalActionDate => ActionDate.ToString(format: "MM/dd/yy");
}