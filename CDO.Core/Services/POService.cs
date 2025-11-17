using CDO.Core.Constants;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class POService : IPOService {
    private readonly INetworkService _network;
    public List<PO> POs { get; private set; } = new();

    public  POService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<PO>>(Endpoints.POs);
        if (data != null) {
            POs = data;
        }
    }

    public Task<List<PO>?> GetAllPOsAsync() {
        return _network.GetAsync<List<PO>>(Endpoints.POs);
    }
}
