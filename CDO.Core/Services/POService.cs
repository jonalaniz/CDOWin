using CDO.Core.Constants;
using CDO.Core.DTOs;
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
    public Task<List<PO>?> GetAllPOsAsync() {
        return _network.GetAsync<List<PO>>(Endpoints.POs);
    }

    public Task<PO?> GetPOAsync(string id) {
        return _network.GetAsync<PO>(Endpoints.PO(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<PO?> CreatePOAsync(NewPODTO dto) {
        return _network.PostAsync<NewPODTO, PO>(Endpoints.POs, dto);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<PO?> UpdatePOAsync(UpdatePODTO dto, string id) {
        return _network.UpdateAsync<UpdatePODTO, PO>(Endpoints.PO(id), dto);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeletePOAsync(string id) {
        return _network.DeleteAsync(Endpoints.PO(id));
    }

}
