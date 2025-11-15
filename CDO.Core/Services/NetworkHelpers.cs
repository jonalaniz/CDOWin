namespace CDO.Core.Services;

public static class NetworkHelpers {
    public static async Task<bool> IsAPIKeyValidAsync(string address, string apiKey) {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        client.BaseAddress = new Uri(address);

        var response = await client.GetAsync("api/");
        return response.IsSuccessStatusCode;
    }
}
