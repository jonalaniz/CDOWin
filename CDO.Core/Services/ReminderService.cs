using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class ReminderService : IReminderService {
    private readonly INetworkService _network;
    public List<Reminder> Reminders { get; private set; } = new();

    public ReminderService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Reminder>>(Endpoints.Reminders);
        if (data != null) {
            Reminders = data;
        }
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Reminder>?> GetAllRemindersAsync() {
        var endpoint = Endpoints.Reminders;
        endpoint += "?includeClients=true";
        return _network.GetAsync<List<Reminder>>(endpoint);
    }

    public Task<Reminder?> GetReminderAsync(int id) {
        return _network.GetAsync<Reminder>(Endpoints.Reminder(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Reminder?> CreateReminderAsync(CreateReminderDTO dto) {
        return _network.PostAsync<CreateReminderDTO, Reminder>(Endpoints.Reminders, dto);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Reminder?> UpdateReminderAsync(UpdateReminderDTO dto, int id) {
        return _network.UpdateAsync<UpdateReminderDTO, Reminder>(Endpoints.Reminder(id), dto);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteReminderAsync(int id) {
        return _network.DeleteAsync(Endpoints.Reminder(id));
    }
}
