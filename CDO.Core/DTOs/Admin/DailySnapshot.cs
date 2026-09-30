namespace CDO.Core.DTOs.Admin;

public record class DailySnapshot(
    DateTime Date,
    List<AdminClientSummary> Clients,
    List<AdminClientNote> Notes,
    List<AdminReminderDetail> Reminders
    );
