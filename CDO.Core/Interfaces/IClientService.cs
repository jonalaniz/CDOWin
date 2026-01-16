using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IClientService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<ClientSummaryDTO>?> GetAllClientSummariesAsync();

    public Task<List<Client>?> GetAllClientsAsync();

    public Task<Client?> GetClientAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result<Client>> CreateClientAsync(CreateClientDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result<Client>> UpdateClientAsync(int id, UpdateClientDTO dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result<bool>> DeleteClientAsync(int id);
}
