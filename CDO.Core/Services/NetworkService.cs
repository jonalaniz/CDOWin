using System.Diagnostics;
using System.Text.Json;

namespace CDO.Core.Services;

public class NetworkService : INetworkService {

    private static NetworkService? _instance;
    public static INetworkService Instance => _instance ?? throw new InvalidOperationException("NetworkService not initialized");

    // Properties
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public NetworkService() {
        _httpClient = new HttpClient();
        _jsonOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
        };
    }

    // Initialize the service with API credentials
    public void Initialize(string baseAddress, string apiKey) {
        if (_instance != null) return;

        Debug.WriteLine($"Base Address: {baseAddress}, API-Key: {apiKey}");

        // Initialize HttpClient
        _httpClient.BaseAddress = new Uri(baseAddress);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

        _instance = this;
    }

    public async Task<T?> GetAsync<T>(string endpoint) {
        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode) {
            return default;
        }

        var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
    }
}
