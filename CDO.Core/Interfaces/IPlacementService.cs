using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;

namespace CDO.Core.Interfaces;

public interface IPlacementService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<PlacementSummary>?> GetAllPlacementSummariesAsync();
    public Task<List<PlacementDetail>?> GetAllPlacementsAsync();

    public Task<PlacementDetail?> GetPlacementAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    // public Task<Placement?> CreatePlacementAsync(PlacementUpdate dto);
    public Task<Result<PlacementDetail>> CreatePlacementAsync(NewPlacement dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result<PlacementDetail>> UpdatePlacementAsync(int id, PlacementUpdate dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result<bool>> DeletePlacementAsync(int id);

}
