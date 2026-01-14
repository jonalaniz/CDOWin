using CDO.Core.Interfaces;
using CDO.Core.Models.Enums;
using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");
Console.WriteLine($"Environment AKI-KEY: {apiKey}");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);
//network.Initialize("http://127.0.0.1:8080", "RISgMANlIwHwqLPvOTDs8ecmz37VyW8O");

IClientService clientService = new ClientService(network);
// IServiceAuthorizationService POService = new POService(network);
//
// var ServiceAuthorizations = await POService.GetAllServiceAuthorizationsAsync();
//
// foreach (var ServiceAuthorization in ServiceAuthorizations) {
//     Console.WriteLine($"State ID: {ServiceAuthorization.Id}");
//     Console.WriteLine($"State Name: {ServiceAuthorization.ClientID}");
// }

//var client = new CreateClientDTO {
//    FirstName = "Jon",
//    LastName = "Alaniz",
//    Counselor = "Someone",
//    City = "San Antonio",
//    State = "TX",
//    Disability = "everything",
//    Ssn = 696969696
//};

// var clients = await clientService.GetAllClientSummariesAsync();
//var client = await clientService.GetClientAsync(2338);

//foreach (var reminder in client.reminders) {
//    Console.WriteLine(reminder.Description);
//}

//Console.WriteLine("Placements");
//Console.WriteLine();
//foreach (var placement in client.placements) {
//    Console.WriteLine(placement.Id);
//    Console.WriteLine(placement.employer.Name);
//    Console.WriteLine(placement.FormattedHireDate);
//    Console.WriteLine(placement.Position);
//}

//Console.WriteLine("Service Authorizations");
//Console.WriteLine();
//foreach (var po in client.pos) {
//    Console.WriteLine(po.Id);
//    Console.WriteLine(po.StartDate);
//    Console.WriteLine(po.EndDate);
//    Console.WriteLine(po.Description);
//}

//foreach (var client in clients) {
//    Console.WriteLine($"Client: {client.Name}");
//    Console.WriteLine($"Printed Counselor Name: {client.CounselorName}");
//}

//var newClient = await clientService.CreateClientAsync(client);
//Console.WriteLine($"New client created: {newClient}");

//var updateClient = new UpdateClientDTO {
//    Race = "lmao"
//};

//var updatedClient = await clientService.UpdateClientAsync(updateClient, newClient.Id);
//Console.WriteLine($"New client updated: {updatedClient}");

//var clientWasDeleted = await clientService.DeleteClientAsync(newClient.Id);
//if (clientWasDeleted == true) {
//    Console.WriteLine($"Client: {newClient.Name} was deleted");
//}

//var llama = new Llama();
//await llama.UpdateModel();
//await llama.Chat();

var value = UM.AllItems();
foreach (var v in value) {
    Console.WriteLine(v.Value);
}