using CDO.Core.Interfaces;
using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");
Console.WriteLine($"Environment AKI-KEY: {apiKey}");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);
//network.Initialize("http://127.0.0.1:8080", "RISgMANlIwHwqLPvOTDs8ecmz37VyW8O");

IServiceAuthorizationService _service = new ServiceAuthorizationService(network);

var invoices = await _service.GetAllServiceAuthorizationsAsync();

foreach(var invoice in invoices) {
    Console.WriteLine($"{invoice.CounselorName}: {invoice.CounselorID}");
}