using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class ClientService : IClientService {
    private readonly INetworkService _network;
    public List<Client> Clients { get; private set; } = new();

    public ClientService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
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

    public Task<List<ClientSummaryDTO>?> GetAllClientSummariesAsync() {
        return _network.GetAsync<List<ClientSummaryDTO>>(Endpoints.ClientSummaries);
    }

    public Task<Client?> GetClientAsync(int id) {
        return _network.GetAsync<Client>(Endpoints.Client(id));
    }

    // -----------------------------
    // POST
    // -----------------------------
    public Task<Client?> CreateClientAsync(CreateClientDTO dto) {
        return _network.PostAsync<CreateClientDTO, Client>(Endpoints.Clients, dto);
    }

    // -----------------------------
    // PATCH
    // -----------------------------
    public Task<Client?> UpdateClientAsync(int id, UpdateClientDTO dto) {
        return _network.UpdateAsync<UpdateClientDTO, Client>(Endpoints.Client(id), dto);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteClientAsync(int id) {
        return _network.DeleteAsync(Endpoints.Client(id));
    }
}
