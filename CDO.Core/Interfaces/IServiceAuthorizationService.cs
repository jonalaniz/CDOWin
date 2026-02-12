using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IServiceAuthorizationService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<Invoice>?> GetAllServiceAuthorizationsAsync();

    public Task<Invoice?> GetServiceAuthorizationAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result> CreateServiceAuthorizationAsync(NewSA dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result> UpdateServiceAuthorizationAsync(int id, SAUpdate dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteServiceAuthorizationAsync(int id);

}
