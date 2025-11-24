using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IReminderService {

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public Task InitializeAsync();

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<Reminder>?> GetAllRemindersAsync();

    public Task<Reminder?> GetReminderAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------

    // -----------------------------
    // PATCH Methods
    // -----------------------------

    // -----------------------------
    // DELETE Methods
    // -----------------------------
}
