using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Models;
using System.Net;

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
    public Task<Reminder?> CreateReminderAsync(CreateReminderDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Reminder?> UpdateReminderAsync(UpdateReminderDTO dto, int id);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteReminderAsync(int id);

}
