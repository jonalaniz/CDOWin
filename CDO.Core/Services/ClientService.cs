using CDO.Core.Models;

namespace CDO.Core.Services;

public class ClientService : IClientService {
    private readonly INetworkService _network;
    public List<Client> Clients { get; private set; } = new();

    public ClientService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Client>>("clients");
        if (data != null) {
            Clients = data;
        }
    }

    public Task<List<Client>?> GetAllClientsAsync() {
        return _network.GetAsync<List<Client>>("clients");
    }
}
