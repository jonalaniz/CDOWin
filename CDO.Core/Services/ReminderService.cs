using CDO.Core.Constants;
using CDO.Core.DTOs.Reminders;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;

namespace CDO.Core.Services;

public class ReminderService : IReminderService {
    private readonly INetworkService _network;

    public ReminderService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<ReminderDetail>?> GetAllRemindersAsync(CancellationToken ct) {
        var endpoint = Endpoints.Reminders;
        endpoint += "?includeClients=true";
        return _network.GetAsync<List<ReminderDetail>>(endpoint, ct);
    }

    public Task<ReminderDetail?> GetReminderAsync(int id) {
        return _network.GetAsync<ReminderDetail>(Endpoints.Reminder(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public async Task<Result<ReminderDetail>> CreateRemindersAsync(NewReminder dto) {
        var result = await _network.PostAsync<NewReminder, ReminderDetail>(Endpoints.Reminders, dto);
        if (!result.IsSuccess) return Result<ReminderDetail>.Fail(TranslateError(result.Error!));
        return Result<ReminderDetail>.Success(result.Value!);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result> UpdateReminderAsync(int id, ReminderUpdate dto) {
        var result = await _network.UpdateAsync(Endpoints.Reminder(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result<ReminderDetail>.Success();
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteReminderAsync(int id) {
        return _network.DeleteAsync(Endpoints.Reminder(id));
    }

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError TranslateError(AppError error) =>
        error.Kind switch {
            ErrorKind.Conflict => error with { Message = "A Reminder with this ID already exists." },
            ErrorKind.Validation => error with { Message = "Invalid data." },
            _ => error
        };
}
