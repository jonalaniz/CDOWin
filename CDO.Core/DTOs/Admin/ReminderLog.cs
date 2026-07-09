namespace CDO.Core.DTOs.Admin;

public record class ReminderLog(
    UserSummary? User,
    DateTime Date,
    DateTime ActionDate,
    string Text,
    bool Complete
    );