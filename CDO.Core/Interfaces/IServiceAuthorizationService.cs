using CDO.Core.DTOs;
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
    public Task<ServiceAuthorization?> CreateServiceAuthorizationAsync(NewServiceAuthorizationDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<ServiceAuthorization?> UpdateServiceAuthorizationAsync(string id, UpdateServiceAuthorizationDTO dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteServiceAuthorizationAsync(string id);

}
