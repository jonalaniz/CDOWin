using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services.Admin;

public class AdminReminderService {
    private readonly INetworkService _network;

    public AdminReminderService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET Methods
    // -----------------------------

    // Returns reminders for the workday
    public Task<List<AdminReminderDetail>?> GetDailyRemindersAsync() {
        return _network.GetAsync<List<AdminReminderDetail>>(Endpoints.AdminReminders);
    }

    // Returns reminders for the given day
    public Task<List<Reminder>?> GetRemindersForDayAsync(string date) {
        var endpoint = Endpoints.AdminReminders + $"?date={date}";
        return _network.GetAsync<List<Reminder>>(endpoint);
    }

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError TranslateError(AppError error) =>
        error.Kind switch {
            ErrorKind.Conflict => error with { Message = "A user with this ID already exists." },
            ErrorKind.Validation => error with { Message = "Invalid data." },
            _ => error
        };
}