using CDO.Core.Constants;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class ReminderService : IReminderService {
    private readonly INetworkService _network;
    public List<Reminder> Reminders { get; private set; } = new();

    public ReminderService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Reminder>>(Endpoints.Reminders);
        if (data != null) {
            Reminders = data;
        }
    }

    public Task<List<Reminder>?> GetAllRemindersAsync() {
        return _network.GetAsync<List<Reminder>>(Endpoints.Reminders);
    }
}
