using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);

IPOService POService = new POService(network);

var POs = await POService.GetAllPOsAsync();

foreach (var PO in POs) {
    Console.WriteLine($"State ID: {PO.id}");
    Console.WriteLine($"State Name: {PO.clientID}");
}

//var llama = new Llama();

//await llama.UpdateModel();

//await llama.Chat();