using CDO.Core.Models;

namespace CDO.Core.Services;

public interface IReminderService {
    public Task InitializeAsync();
    public Task<List<Reminder>?> GetAllRemindersAsync();
}
