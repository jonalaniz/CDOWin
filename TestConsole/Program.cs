using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);

IClientService clientService = new ClientService(network);

var clients = await clientService.GetAllClientsAsync();

foreach (var client in clients) {
    Console.WriteLine($"Client ID: {client.id}");
    Console.WriteLine($"Client Name: {client.firstName} {client.lastName}");
}

//var llama = new Llama();

//await llama.UpdateModel();

//await llama.Chat();