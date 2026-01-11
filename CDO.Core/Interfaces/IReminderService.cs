using CDO.Core.DTOs;
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
    //public Task<Reminder?> CreateReminderAsync(CreateReminderDTO dto);
    public Task<Result<Reminder>> CreateRemindersAsync(CreateReminderDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result<Reminder>> UpdateReminderAsync(int id, UpdateReminderDTO dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteReminderAsync(int id);

}
