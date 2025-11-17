using CDO.Core.Constants;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class PoService : IPoService {
    private readonly INetworkService _network;
    public List<Po> Pos { get; private set; } = new();

    public  PoService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Po>>(Endpoints.PO);
        if (data != null) {
            Pos = data;
        }
    }

    public Task<List<Po>?> GetAllPosAsync() {
        return _network.GetAsync<List<Po>>(Endpoints.PO);
    }
}
