using System.Diagnostics;
using System.Net.Http.Json;
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

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public void Initialize(string baseAddress, string apiKey) {
        if (_instance != null) return;

        // Initialize HttpClient
        _httpClient.BaseAddress = new Uri(baseAddress);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

        _instance = this;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public async Task<T?> GetAsync<T>(string endpoint) {
        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode) {
            return default;
        }

        var stream = await response.Content.ReadAsStreamAsync();
        try {
            return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
        } catch (JsonException ex) {
            Debug.WriteLine("JSON DESERIALIZATION ERROR:");
            Debug.WriteLine($"Message: {ex.Message}");
            Debug.WriteLine($"Path: {ex.Path}");
            Debug.WriteLine($"LineNumber: {ex.LineNumber}");
            Debug.WriteLine($"BytePositionInLine: {ex.BytePositionInLine}");

            throw;
        }
    }

    // -----------------------------
    // POST
    // -----------------------------
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest body) {
        var content = JsonContent.Create(body);
        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    // -----------------------------
    // PATCH
    // -----------------------------
    public async Task<TResponse?> UpdateAsync<TRequest, TResponse>(string endpoint, TRequest body) {
        var content = JsonContent.Create(body);
        var response = await _httpClient.PatchAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    // -----------------------------
    // DELETE
    // -----------------------------
    public async Task<bool> DeleteAsync(string endpoint) {
        var response = await _httpClient.DeleteAsync(endpoint);

        // If the API is written to always return 204 No Content,
        // success = true is safe here.
        if (response.IsSuccessStatusCode)
            return true;

        // You can include logs here if desired
        Debug.WriteLine($"DELETE failed: {response.StatusCode}");
        return false;
    }
}
