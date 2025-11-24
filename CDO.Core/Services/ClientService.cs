using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class ClientService : IClientService {
    private readonly INetworkService _network;
    public List<Client> Clients { get; private set; } = new();

    public ClientService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Client>>(Endpoints.Clients);
        if (data != null) {
            Clients = data;
        }
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Client>?> GetAllClientsAsync() {
        return _network.GetAsync<List<Client>>(Endpoints.Clients);
    }

    public Task<Client?> GetClientAsync(int id) {
        return _network.GetAsync<Client> (Endpoints.Client(id));
    }
    
    // -----------------------------
    // POST
    // -----------------------------

    public Task<Client?> CreateClientAsync(CreateClientDTO dto) {
        return _network.PostAsync<CreateClientDTO, Client> (Endpoints.Clients, dto);
    }
}
