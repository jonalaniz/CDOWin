using CDO.Core.Interfaces;
using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");
Console.WriteLine($"Environment AKI-KEY: {apiKey}");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);
//network.Initialize("http://127.0.0.1:8080", "RISgMANlIwHwqLPvOTDs8ecmz37VyW8O");

ICounselorService _counselorService = new CounselorService(network);
var counserlorSummaries = await _counselorService.GetAllCounselorSummariesAsync();
foreach (var summary in counserlorSummaries) {
    Console.WriteLine($"{summary.Name}: {summary.Id}");
}

var counselor = await _counselorService.GetCounselorAsync(2);
Console.WriteLine($"Counselor: {counselor.Name}");
foreach (var invoice in counselor.Invoices) {
    Console.WriteLine(invoice.Id);
}

foreach (var client in counselor.Clients) {
    Console.WriteLine(client.Name);
}