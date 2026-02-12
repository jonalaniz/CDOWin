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
    Task<Result> PostAsync<T>(string endpoint, T body);

    // -----------------------------
    // PATCH
    // -----------------------------
    Task<Result> UpdateAsync<T>(string endpoint, T body);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    Task<Result> DeleteAsync(string endpoint);
}
