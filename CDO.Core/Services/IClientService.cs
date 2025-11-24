using CDO.Core.Models;
using CDO.Core.DTOs;

namespace CDO.Core.Services;

public interface IClientService {
    public Task InitializeAsync();
    
    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Client>?> GetAllClientsAsync();
    
    public Task<Client?> GetClientAsync(int id);

    // -----------------------------
    // POST
    // -----------------------------
    public Task<Client?> CreateClientAsync(CreateClientDTO dto);
    
    // -----------------------------
    // PATCH
    // -----------------------------
    public Task<Client?> UpdateClientAsync(UpdateClientDTO dto, int id);
}
