namespace CDO.Core.DTOs.Admin;

public record class ReminderLog(
    UserSummary? User,
    DateTime Date,
    DateTime ActionDate,
    string Text,
    bool Complete
    ) {
    public string LocalDate => Date.ToString(format: "MM/dd/yyyy");
    public string LocalActionDate => ActionDate.ToString(format: "MM/dd/yyyy");
}