using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.Models;

namespace CDO.Core.Interfaces.Admin;

public interface IAdminService {

    // -----------------------------
    // GET Methods
    // -----------------------------

    // Returns client summaries for given date (or today if no date present)
    public Task<List<AdminClientSummary>?> GetDailyClientSummariesAsync(string? date);

    // Returns complete client records for auditing/export purposes
    public Task<List<AdminClientDetail>?> GetClientAuditRecordsAsync();

    public Task<List<AdminClientSummary>?> GetStaleClients();

    public Task<List<ClientNote>?> GetDailyNotesAsync(string? date);

    public Task<List<ClientNote>?> GetClientNotesByAuthorAsync(string author);

    public Task<List<Reminder>?> GetDailyRemindersAsync(string? date);
}