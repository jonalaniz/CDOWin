using CDO.Core.DTOs;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IClientService {

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public Task InitializeAsync();

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<Client>?> GetAllClientsAsync();

    public Task<Client?> GetClientAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Client?> CreateClientAsync(CreateClientDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Client?> UpdateClientAsync(UpdateClientDTO dto, int id);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
}
