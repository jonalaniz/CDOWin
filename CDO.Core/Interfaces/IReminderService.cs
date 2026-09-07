using CDO.Core.DTOs.Reminders;
using CDO.Core.ErrorHandling;

namespace CDO.Core.Interfaces;

public interface IReminderService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<ReminderDetail>?> GetAllRemindersAsync(CancellationToken ct);

    public Task<ReminderDetail?> GetReminderAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result<ReminderDetail>> CreateRemindersAsync(NewReminder dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result> UpdateReminderAsync(int id, ReminderUpdate dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteReminderAsync(int id);

}
