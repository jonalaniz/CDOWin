using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
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
    public async Task<Result<Reminder>> CreateRemindersAsync(CreateReminderDTO dto) {
        var result = await _network.PostAsync<CreateReminderDTO, Reminder>(Endpoints.Reminders, dto);
        if (!result.IsSuccess) return Result<Reminder>.Fail(TranslateError(result.Error!));
        return Result<Reminder>.Success(result.Value!);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result<Reminder>> UpdateReminderAsync(int id, UpdateReminderDTO dto) {
        var result = await _network.UpdateAsync<UpdateReminderDTO, Reminder>(Endpoints.Reminder(id), dto);
        if (!result.IsSuccess) return Result<Reminder>.Fail(TranslateError(result.Error!));
        return Result<Reminder>.Success(result.Value!);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteReminderAsync(int id) {
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
