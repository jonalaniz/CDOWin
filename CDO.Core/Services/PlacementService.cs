using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
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
    public Task<List<PlacementSummaryDTO>?> GetAllPlacementSummariesAsync() {
        return _network.GetAsync<List<PlacementSummaryDTO>>(Endpoints.PlacementSummaries);
    }

    public Task<List<Placement>?> GetAllPlacementsAsync() {
        return _network.GetAsync<List<Placement>>(Endpoints.Placements);
    }

    public Task<Placement?> GetPlacementAsync(string id) {
        return _network.GetAsync<Placement>(Endpoints.Placement(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public async Task<Result<Placement>> CreatePlacementAsync(PlacementDTO dto) {
        var result = await _network.PostAsync<PlacementDTO, Placement>(Endpoints.Placements, dto);
        if (!result.IsSuccess) return Result<Placement>.Fail(TranslateError(result.Error!));
        return Result<Placement>.Success(result.Value!);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result<Placement>> UpdatePlacementAsync(string id, PlacementDTO dto) {
        var result = await _network.UpdateAsync<PlacementDTO, Placement>(Endpoints.Placement(id), dto);
        if (!result.IsSuccess) return Result<Placement>.Fail(TranslateError(result.Error!));
        return Result<Placement>.Success(result.Value!);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result<bool>> DeletePlacementAsync(string id) {
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
