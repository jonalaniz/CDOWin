using CDO.Core.Constants;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class POService : IPOService {
    private readonly INetworkService _network;
    public List<PO> POs { get; private set; } = new();

    public POService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<PO>>(Endpoints.POs);
        if (data != null) {
            POs = data;
        }
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<PO>?> LoadPOsAsync() {
        return _network.GetAsync<List<PO>>(Endpoints.POs);
    }

    public Task<PO?> LoadPOAsync(string id) {
        return _network.GetAsync<PO>(Endpoints.PO(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------

    // -----------------------------
    // PATCH Methods
    // -----------------------------

    // -----------------------------
    // DELETE Methods
    // -----------------------------
}
