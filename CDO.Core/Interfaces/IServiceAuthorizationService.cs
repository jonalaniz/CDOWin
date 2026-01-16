using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IServiceAuthorizationService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<ServiceAuthorization>?> GetAllServiceAuthorizationsAsync();

    public Task<ServiceAuthorization?> GetServiceAuthorizationAsync(string id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result<ServiceAuthorization>> CreateServiceAuthorizationAsync(CreateSADTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result<ServiceAuthorization>> UpdateServiceAuthorizationAsync(string id, UpdateServiceAuthorizationDTO dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result<bool>> DeleteServiceAuthorizationAsync(string id);

}
