using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;

namespace CDO.Core.Services.Admin;

public class AdminClientService {
    private readonly INetworkService _network;

    public AdminClientService(INetworkService network) {
        _network = network;
    }

    // =============================
    // GET Methods (Cached)
    // =============================

    /// <summary>
    /// Returns recently created and updated clients for today, or a specific date if provided.
    /// </summary>
    /// <param name="date">An optional ISO 8601 date. Defaults to today if null.</param>
    /// <returns>A list of <see cref="AdminClientSummary"/>.</returns>
    public Task<List<AdminClientSummary>?> GetRecentClientSummariesAsync(string? date) {
        var endpoint = Endpoints.AdminClients;
        if (date != null) endpoint += $"?date={date}";
        return _network.GetAsync<List<AdminClientSummary>>(endpoint);
    }

    /// <summary>
    /// Returns all clients not updated in the past week.
    /// </summary>
    /// <returns>A list of AdminClientSummaries</returns>
    public Task<List<AdminClientSummary>?> GetStaleClientsAsync() {
        return _network.GetAsync<List<AdminClientSummary>>(Endpoints.AdminStaleClients);
    }

    /// <summary>
    /// Returns client notes for the workday.
    /// </summary>
    /// <returns>A list of <see cref="ClientNote"/>.</returns>
    public Task<List<AdminClientNote>?> GetRecentClientNotesAsync() {
        return _network.GetAsync<List<AdminClientNote>>(Endpoints.AdminNotes);
    }

    // =============================
    // GET — Audit / Export (no cache_
    // =============================

    /// <summary>
    /// Returns all clients in database for audit/export purposes.
    /// </summary>
    /// <returns>A list of AdminClientDetail</returns>
    public Task<List<AdminClientDetail>?> GetAllClientRecordsAsync() {
        return _network.GetAsync<List<AdminClientDetail>>(Endpoints.AdminAllClients);
    }

    /// <summary>
    /// Returns client notes for the given date.
    /// </summary>
    /// <param name="date">An ISO 8601 date string.</param>
    /// <returns>A list of <see cref="ClientNote"/>.</returns>
    public Task<List<ClientNote>?> GetClientNotesForDateAsync(string date) {
        var endpoint = Endpoints.AdminNotes + $"?date={date}";
        return _network.GetAsync<List<ClientNote>>(endpoint);
    }

    /// <summary>
    /// Returns client notes for the given author.
    /// </summary>
    /// <param name="author">The author's username.</param>
    /// <returns>A list of <see cref="ClientNote"/>.</returns>
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
