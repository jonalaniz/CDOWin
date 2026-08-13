namespace CDO.Core.DTOs.Reminders;

public record class NewReminder(
    int ClientID,
    DateTime ActionDate,
    string Text
);