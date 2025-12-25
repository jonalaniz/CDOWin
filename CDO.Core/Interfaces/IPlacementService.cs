using CDO.Core.DTOs;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IPlacementService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<Placement>?> GetAllPlacementsAsync();

    public Task<Placement?> GetPlacementAsync(string id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Placement?> CreatePlacementAsync(PlacementDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Placement?> UpdatePlacementAsync(string id, PlacementDTO dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeletePlacementAsync(string id);

}
