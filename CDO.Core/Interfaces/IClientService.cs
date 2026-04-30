using CDO.Core.DTOs.Clients;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.ErrorHandling;

namespace CDO.Core.Interfaces;

public interface IClientService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<ClientSummary>?> GetAllClientSummariesAsync();

    public Task<List<ClientDetail>?> GetAllClientsAsync();

    public Task<ClientDetail?> GetClientAsync(int id, CancellationToken ct = default);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result<ClientDetail>> CreateClientAsync(NewClient dto);

    public Task<Result<ClientNote>> CreateClientNoteAsync(NewNote dto, int clientId);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result> UpdateClientAsync(int id, ClientUpdate dto);

    public Task<Result> UpdateClientNote(NewNote dto, int clientId, int noteId);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteClientAsync(int id);

    public Task<Result> DeleteClientNoteAsync(int clientId, int noteId);
}
