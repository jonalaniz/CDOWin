using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class PlacementService : IPlacementService {
    private readonly INetworkService _network;
    public List<Placement> Placements { get; private set; } = new();

    public PlacementService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Placement>?> GetAllPlacementsAsync() {
        return _network.GetAsync<List<Placement>>(Endpoints.Placements);
    }

    public Task<Placement?> GetPlacementAsync(string id) {
        return _network.GetAsync<Placement>(Endpoints.Placement(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Placement?> CreatePlacementAsync(PlacementDTO dto) {
        return _network.PostAsync<PlacementDTO, Placement>(Endpoints.Placements, dto);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Placement?> UpdatePlacementAsync(string id, PlacementDTO dto) {
        return _network.UpdateAsync<PlacementDTO, Placement>(Endpoints.Placement(id), dto);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeletePlacementAsync(string id) {
        return _network.DeleteAsync(Endpoints.Placement(id));
    }

}
