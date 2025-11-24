using CDO.Core.Models;
using CDO.Core.DTOs;

namespace CDO.Core.Services;

public interface IClientService {
    public Task InitializeAsync();
    
    public Task<List<Client>?> GetAllClientsAsync();
    
    public Task<Client?> GetClientAsync(int id);

    public Task<Client?> CreateClientAsync(CreateClientDTO dto);
}
