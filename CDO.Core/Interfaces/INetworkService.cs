using CDO.Core.ErrorHandling;

namespace CDO.Core.Interfaces;

public interface INetworkService {

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    void Initialize(string baseAddress, string apiKey);

    // -----------------------------
    // GET
    // -----------------------------
    Task<T?> GetAsync<T>(string endpoint);

    // -----------------------------
    // POST
    // -----------------------------
    Task<Result<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest body);

    // -----------------------------
    // PATCH
    // -----------------------------
    Task<Result<TResponse>> UpdateAsync<TRequest, TResponse>(string endpoint, TRequest body);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    Task<Result<bool>> DeleteAsync(string endpoint);
}
