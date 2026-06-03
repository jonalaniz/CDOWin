using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services.Admin;
using CDO.Core.ErrorHandling;

public class AdminService {
    private readonly INetworkService _network;


    public AdminService(INetworkService network) {
        _network = network;
    }
    
    // -----------------------------
    // GET Methods
    // -----------------------------
    
    // Returns client summaries for given date (or today if no date present)
    public Task<List<AdminClientSummary>?> GetDailyClientSummariesAsync(string? date) {
        var endpoint = Endpoints.AdminClients;
        if (date != null) { endpoint += $"?date={date}"; }
        return _network.GetAsync<List<AdminClientSummary>>(endpoint);
    }

    // Returns complete client records for auditing/export purposes
    public Task<List<AdminClientDetail>?> GetClientAuditRecordsAsync() {
        return _network.GetAsync<List<AdminClientDetail>>(Endpoints.AdminAllClients);
    }

    public Task<List<AdminClientSummary>?> GetStaleClients() {
        return _network.GetAsync<List<AdminClientSummary>>(Endpoints.AdminStaleClients);
    }

    // Returns client notes for given date (or today if no date present)
    public Task<List<ClientNote>?> GetDailyNotesAsync(string? date) {
        var endpoint = Endpoints.AdminClients;
        if (date != null) { endpoint += $"?date={date}"; }
        return _network.GetAsync<List<ClientNote>>(endpoint);
    }

    public Task<List<ClientNote>?> GetClientNotesByAuthorAsync(string author) {
        return _network.GetAsync<List<ClientNote>>(Endpoints.AdminUserNotes(author));
    }

    public Task<List<Reminder>?> GetDailyRemindersAsync(string? date) {
        return _network.GetAsync<List<Reminder>>(Endpoints.AdminReminders);
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