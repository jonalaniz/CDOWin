using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);

IEmployerService employerService = new EmployerService(network);

var employers = await employerService.GetAllEmployersAsync();

foreach (var employer in employers) {
    Console.WriteLine($"State ID: {employer.id}");
    Console.WriteLine($"State Name: {employer.name}");
}

//var llama = new Llama();

//await llama.UpdateModel();

//await llama.Chat();