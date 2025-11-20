using CDO.Core.Constants;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class StateService : IStateService {
    private readonly INetworkService _network;
    public List<State> States { get; private set; } = new();

    public StateService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<State>>("/api/states/");
        if (data != null) {
            States = data;
        }
    }

    public Task<List<State>?> GetAllStatesAsync() {
        return _network.GetAsync<List<State>>("/api/states/");
    }

    public Task<State?> GetStateAsync(int id) {
        return _network.GetAsync<State>(Endpoints.State(id));
    }
}
