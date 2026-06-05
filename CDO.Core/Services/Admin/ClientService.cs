using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;

namespace CDO.Core.Services.Admin;

public class ClientService {
    private readonly INetworkService _network;

    public ClientService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET Methods
    // -----------------------------

    // Retruns client summaries for the workday
    public Task<List<AdminClientSummary>?> GetRecentClientSummariesAsync() {
        return _network.GetAsync<List<AdminClientSummary>>(Endpoints.AdminClients);
    }

    // Returns client summaries for given date
    public Task<List<AdminClientSummary>?> GetClientSummariesForDateAsync(string date) {
        var endpoint = Endpoints.AdminClients + $"?date={date}";
        return _network.GetAsync<List<AdminClientSummary>>(endpoint);
    }

    // Returns complete client records for auditing/export purposes
    public Task<List<AdminClientDetail>?> GetAllClientRecordsAsync() {
        return _network.GetAsync<List<AdminClientDetail>>(Endpoints.AdminAllClients);
    }

    // Returns clients not updated in the past week
    public Task<List<AdminClientSummary>?> GetStaleClientsAsync() {
        return _network.GetAsync<List<AdminClientSummary>>(Endpoints.AdminStaleClients);
    }

    // Returns client notes for the workday
    public Task<List<ClientNote>?> GetRecentClientNotesAsync() {
        return _network.GetAsync<List<ClientNote>>(Endpoints.AdminNotes);
    }

    // Returns client notes for the given date
    public Task<List<ClientNote>?> GetClientNotesForDateAsync(string date) {
        var endpoint = Endpoints.AdminNotes + $"?date={date}";
        return _network.GetAsync<List<ClientNote>>(endpoint);
    }

    // Returns client notes for the given author
    public Task<List<ClientNote>?> GetClientNotesByAuthorAsync(string author) {
        return _network.GetAsync<List<ClientNote>>(Endpoints.AdminUserNotes(author));
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
