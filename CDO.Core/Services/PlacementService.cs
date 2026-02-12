using CDO.Core.Constants;
using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;

namespace CDO.Core.Services;

public class PlacementService : IPlacementService {
    private readonly INetworkService _network;
    public List<PlacementDetail> Placements { get; private set; } = new();

    public PlacementService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<PlacementSummary>?> GetAllPlacementSummariesAsync() {
        return _network.GetAsync<List<PlacementSummary>>(Endpoints.PlacementSummaries);
    }

    public Task<List<PlacementDetail>?> GetAllPlacementsAsync() {
        return _network.GetAsync<List<PlacementDetail>>(Endpoints.Placements);
    }

    public Task<PlacementDetail?> GetPlacementAsync(int id) {
        return _network.GetAsync<PlacementDetail>(Endpoints.Placement(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public async Task<Result> CreatePlacementAsync(NewPlacement dto) {
        var result = await _network.PostAsync(Endpoints.Placements, dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result> UpdatePlacementAsync(int id, PlacementUpdate dto) {
        var result = await _network.UpdateAsync(Endpoints.Placement(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeletePlacementAsync(int id) {
        return _network.DeleteAsync(Endpoints.Placement(id));
    }

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError TranslateError(AppError error) =>
        error.Kind switch {
            ErrorKind.Conflict => error with { Message = "A Placement with this ID already exists." },
            ErrorKind.Validation => error with { Message = "Invalid data." },
            _ => error
        };
}
