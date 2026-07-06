using CDO.Core.Constants;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class StateService : IStateService {
    private readonly INetworkService _network;

    public StateService(INetworkService network) {
        _network = network;
    }

    public Task<List<State>?> GetAllStatesAsync() {
        return _network.GetAsync<List<State>>(Endpoints.States);
    }
}
