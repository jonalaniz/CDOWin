using CDO.Core.DTOs.Reminders;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IReminderService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<Reminder>?> GetAllRemindersAsync();

    public Task<Reminder?> GetReminderAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result> CreateRemindersAsync(NewReminder dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result> UpdateReminderAsync(int id, ReminderUpdate dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteReminderAsync(int id);

}
