using CDO.Core.Models;

namespace CDO.Core.Services;

public interface IClientService {
    public Task<List<Client>?> GetAllClientsAsync();
    public Task<Client?> GetClientAsync(int id);
}
