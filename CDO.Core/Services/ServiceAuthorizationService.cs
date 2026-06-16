using CDO.Core.Constants;
using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;

namespace CDO.Core.Services;

public class ServiceAuthorizationService : IServiceAuthorizationService {
    private readonly INetworkService _network;

    public ServiceAuthorizationService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<SASummary>?> GetAllServiceAuthorizationsAsync() {
        return _network.GetAsync<List<SASummary>>(Endpoints.ServiceAuthorizations);
    }

    public Task<SADetail?> GetServiceAuthorizationAsync(int id) {
        return _network.GetAsync<SADetail>(Endpoints.ServiceAuthorization(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public async Task<Result<SADetail>> CreateServiceAuthorizationAsync(NewSA dto) {
        var result = await _network.PostAsync<NewSA, SADetail>(Endpoints.ServiceAuthorizations, dto);
        if (!result.IsSuccess) return Result<SADetail>.Fail(TranslateError(result.Error!));
        return Result<SADetail>.Success(result.Value!);
    }

    public Task<Result>MarkSABilled(int id) {
        return _network.PostAsync(Endpoints.SAMarkBilled(id));
    }

    public Task<Result>MarkSAUnbilled(int id) {
        return _network.PostAsync(Endpoints.SAMarkUnbilled(id));
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result> UpdateServiceAuthorizationAsync(int id, SAUpdate dto) {
        var result = await _network.UpdateAsync(Endpoints.ServiceAuthorization(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result<SADetail>.Success();
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
