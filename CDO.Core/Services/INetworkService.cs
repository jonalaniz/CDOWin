namespace CDO.Core.Services;

public interface INetworkService {
    /// <summary> Initialize the network service with base address and API Key</summary>
    void Initialize(string baseAddress, string apiKey);

    /// <summary>Perform a GET request to the specificed endpoint and deserialize the result</summary>
    Task<T?> GetAsync<T>(string endpoint);
}
