using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class StateService : IStateService {
    private readonly INetworkService _network;
    public List<State> States { get; private set; } = new();

    public StateService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<State>>(Endpoints.States);
        if (data != null) {
            States = data;
        }
    }

    public Task<List<State>?> GetAllStatesAsync() {
        return _network.GetAsync<List<State>>(Endpoints.States);
    }

    public Task<State?> GetStateAsync(int id) {
        return _network.GetAsync<State>(Endpoints.State(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<State?> CreateStateAsync(CreateStateDTO dto) {
        return _network.PostAsync<CreateStateDTO, State>(Endpoints.States, dto);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<State?> UpdateStateAsync(UpdateStateDTO dto, int id) {
        return _network.UpdateAsync<UpdateStateDTO, State>(Endpoints.State(id), dto);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteStateAsync(int id) {
        return _network.DeleteAsync(Endpoints.State(id));
    }

}
