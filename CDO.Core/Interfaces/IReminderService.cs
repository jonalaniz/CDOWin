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
    //public Task<Reminder?> CreateReminderAsync(NewReminder dto);
    public Task<Result<Reminder>> CreateRemindersAsync(NewReminder dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result<Reminder>> UpdateReminderAsync(int id, ReminderUpdate dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result<bool>> DeleteReminderAsync(int id);

}
