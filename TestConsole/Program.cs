using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");
Console.WriteLine($"Environment AKI-KEY: {apiKey}");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);
//network.Initialize("http://127.0.0.1:8080", "RISgMANlIwHwqLPvOTDs8ecmz37VyW8O");

//IClientService _service = new ClientService(network);

//var clients = await _service.GetAllClientsAsync();

//var size = 0;
//var note = "";

//foreach (var client in clients) {
//    if (client.ClientNotes.Length > size)
//    {
//        size = client.ClientNotes.Length;
//        note = client.ClientNotes;
//    }
//}

//Console.WriteLine($"Longest note: {note}");
//Console.WriteLine($"Size: {size}");