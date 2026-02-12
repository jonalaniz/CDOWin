using CDO.Core.Constants;
using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class ServiceAuthorizationService : IServiceAuthorizationService {
    private readonly INetworkService _network;
    public List<Invoice> ServiceAuthorizations { get; private set; } = new();

    public ServiceAuthorizationService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Invoice>?> GetAllServiceAuthorizationsAsync() {
        return _network.GetAsync<List<Invoice>>(Endpoints.ServiceAuthorizations);
    }

    public Task<Invoice?> GetServiceAuthorizationAsync(int id) {
        return _network.GetAsync<Invoice>(Endpoints.ServiceAuthorization(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public async Task<Result> CreateServiceAuthorizationAsync(NewSA dto) {
        var result = await _network.PostAsync(Endpoints.ServiceAuthorizations, dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result> UpdateServiceAuthorizationAsync(int id, SAUpdate dto) {
        var result = await _network.UpdateAsync(Endpoints.ServiceAuthorization(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result<Invoice>.Success();
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteServiceAuthorizationAsync(int id) {
        return _network.DeleteAsync(Endpoints.ServiceAuthorization(id));
    }

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError TranslateError(AppError error) =>
        error.Kind switch {
            ErrorKind.Conflict => error with { Message = "An SA with this ID already exists." },
            ErrorKind.Validation => error with { Message = "Invalid data." },
            _ => error
        };
}
