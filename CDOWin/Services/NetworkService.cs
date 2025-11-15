using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CDOWin.Services {
    public class NetworkService {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public NetworkService(string baseUrl, string apiKey = null) {
            _httpClient = new HttpClient {
                BaseAddress = new Uri(baseUrl)
            };

            if (!string.IsNullOrEmpty(apiKey)) {
                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            }

            _jsonOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T?> GetAsync<T>(string endpoint) {
            var response = await _httpClient.GetAsync(endpoint);

            if(!response.IsSuccessStatusCode) {
                return default;
            }

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
        }
    }
}
