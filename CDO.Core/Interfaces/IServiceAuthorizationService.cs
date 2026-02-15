using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;

namespace CDO.Core.Interfaces;

public interface IServiceAuthorizationService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<InvoiceSummary>?> GetAllServiceAuthorizationsAsync();

    public Task<InvoiceDetail?> GetServiceAuthorizationAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result<InvoiceDetail>> CreateServiceAuthorizationAsync(NewSA dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result> UpdateServiceAuthorizationAsync(int id, SAUpdate dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteServiceAuthorizationAsync(int id);

}
