namespace CDO.Core.Services;

public interface INetworkService {
    /// <summary> Initialize the network service with base address and API Key</summary>
    void Initialize(string baseAddress, string apiKey);

    // -----------------------------
    // GET
    // -----------------------------
    Task<T?> GetAsync<T>(string endpoint);

    // -----------------------------
    // POST
    // -----------------------------
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest body);

    // -----------------------------
    // PATCH
    // -----------------------------
    Task<TResponse?> UpdateAsync<TRequest, TResponse>(string endpoint, TRequest body);
}
