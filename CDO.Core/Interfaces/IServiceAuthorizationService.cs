using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IServiceAuthorizationService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<Invoice>?> GetAllServiceAuthorizationsAsync();

    public Task<Invoice?> GetServiceAuthorizationAsync(string id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result<Invoice>> CreateServiceAuthorizationAsync(CreateInvoiceDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result<Invoice>> UpdateServiceAuthorizationAsync(string id, UpdateInvoiceDTO dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result<bool>> DeleteServiceAuthorizationAsync(string id);

}
