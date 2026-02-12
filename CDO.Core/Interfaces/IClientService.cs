using CDO.Core.DTOs.Clients;
using CDO.Core.ErrorHandling;

namespace CDO.Core.Interfaces;

public interface IClientService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<ClientSummary>?> GetAllClientSummariesAsync();

    public Task<List<ClientDetail>?> GetAllClientsAsync();

    public Task<ClientDetail?> GetClientAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result<ClientDetail>> CreateClientAsync(NewClient dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result> UpdateClientAsync(int id, ClientUpdate dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteClientAsync(int id);
}
