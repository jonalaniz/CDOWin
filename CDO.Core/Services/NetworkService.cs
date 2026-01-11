using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Serialization;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CDO.Core.Services;

public class NetworkService : INetworkService {

    private static NetworkService? _instance;
    public static INetworkService Instance => _instance ?? throw new InvalidOperationException("NetworkService not initialized");

    // Properties
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly String MediaType = "application/json";

    public NetworkService() {
        _httpClient = new HttpClient();
        _jsonOptions = new JsonSerializerOptions {
            TypeInfoResolver = SourceGenerationContext.Default,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
        try {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode) {
                return default;
            }

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
        } catch (JsonException ex) {
            Debug.WriteLine("JSON DESERIALIZATION ERROR:");
            Debug.WriteLine($"Message: {ex.Message}");
            Debug.WriteLine($"Path: {ex.Path}");
            Debug.WriteLine($"LineNumber: {ex.LineNumber}");
            Debug.WriteLine($"BytePositionInLine: {ex.BytePositionInLine}");

            throw;
        } catch (Exception ex) {
            Debug.WriteLine(ex.Message);
            throw;
        }
    }

    // -----------------------------
    // POST
    // -----------------------------
    public async Task<Result<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest body) {
        try {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            var content = new StringContent(json, encoding: Encoding.UTF8, MediaType);
            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode) {
                var data = await response.Content.ReadFromJsonAsync<TResponse>();
                return Result<TResponse>.Success(data!);
            }

            return Result<TResponse>.Fail(MapHttpError(response.StatusCode));
        } catch (TaskCanceledException ex) {
            return Result<TResponse>.Fail(new AppError(ErrorKind.Timeout, "The request timd out.", null, ex));
        } catch (HttpRequestException ex) {
            return Result<TResponse>.Fail(new AppError(ErrorKind.Network, "Unable to reach the server.", null, ex));
        } catch (Exception ex) {
            return Result<TResponse>.Fail(new AppError(ErrorKind.Unknown, "Unexpected error occurred.", null, ex));
        }
    }

    // -----------------------------
    // PATCH
    // -----------------------------
    //public async Task<TResponse?> UpdateAsync<TRequest, TResponse>(string endpoint, TRequest body) {
    //    var json = JsonSerializer.Serialize(body, _jsonOptions);
    //    var content = new StringContent(json, encoding: Encoding.UTF8, MediaType);

    //    try {
    //        var response = await _httpClient.PatchAsync(endpoint, content);
    //        response.EnsureSuccessStatusCode();

    //        return await response.Content.ReadFromJsonAsync<TResponse>();
    //    } catch (HttpRequestException ex) {
    //        Debug.WriteLine("HTTP Request Exception:");
    //        Debug.WriteLine($"Message: {ex.Message}");
    //        throw;
    //    }
    //}

    public async Task<Result<TResponse>> UpdateAsync<TRequest, TResponse>(string endpoint, TRequest body) {
        try {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            var content = new StringContent(json, encoding: Encoding.UTF8, MediaType);
            var response = await _httpClient.PatchAsync(endpoint, content);

            if(response.IsSuccessStatusCode) {
                var data = await response.Content.ReadFromJsonAsync<TResponse>();
                return Result<TResponse>.Success(data!);
            }

            return Result<TResponse>.Fail(MapHttpError(response.StatusCode));
        } catch (TaskCanceledException ex) {
            return Result<TResponse>.Fail(new AppError(ErrorKind.Timeout, "The request timd out.", null, ex));
        } catch (HttpRequestException ex) {
            return Result<TResponse>.Fail(new AppError(ErrorKind.Network, "Unable to reach the server.", null, ex));
        } catch (Exception ex) {
            return Result<TResponse>.Fail(new AppError(ErrorKind.Unknown, "Unexpected error occurred.", null, ex));
        }
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

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError MapHttpError(HttpStatusCode status) => status switch {
        HttpStatusCode.BadRequest => new(ErrorKind.Validation, "Invalid request.", (int)status),
        HttpStatusCode.Unauthorized => new(ErrorKind.Unauthorized, "Invalid API Key.", (int)status),
        HttpStatusCode.Forbidden => new(ErrorKind.Forbidden, "Access denied.", (int)status),
        HttpStatusCode.Conflict => new(ErrorKind.Conflict, "The requested item already exists.", (int)status),
        HttpStatusCode.InternalServerError => new(ErrorKind.Server, "Invalid request.", (int)status),
        _ => new(ErrorKind.Unknown, "Requet failed.", (int)status)
    };
}
