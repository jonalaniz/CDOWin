using CDO.Core.Interfaces;
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
//     Console.WriteLine($"State ID: {ServiceAuthorization.id}");
//     Console.WriteLine($"State Name: {ServiceAuthorization.clientID}");
// }

//var client = new CreateClientDTO {
//    firstName = "Jon",
//    lastName = "Alaniz",
//    counselor = "Someone",
//    city = "San Antonio",
//    state = "TX",
//    disability = "everything",
//    ssn = 696969696
//};

// var clients = await clientService.GetAllClientSummariesAsync();
var client = await clientService.GetClientAsync(2338);

foreach (var reminder in client.reminders) {
    Console.WriteLine(reminder.description);
}

Console.WriteLine("Referrals");
Console.WriteLine();
foreach (var referral in client.referrals) {
    Console.WriteLine(referral.id);
    Console.WriteLine(referral.employer.name);
    Console.WriteLine(referral.formattedHireDate);
    Console.WriteLine(referral.position);
}

Console.WriteLine("Service Authorizations");
Console.WriteLine();
foreach (var po in client.pos) {
    Console.WriteLine(po.id);
    Console.WriteLine(po.startDate);
    Console.WriteLine(po.endDate);
    Console.WriteLine(po.description);
}

//foreach (var client in clients) {
//    Console.WriteLine($"Client: {client.name}");
//    Console.WriteLine($"Printed Counselor name: {client.counselorName}");
//}

//var newClient = await clientService.CreateClientAsync(client);
//Console.WriteLine($"New client created: {newClient}");

//var updateClient = new UpdateClientDTO {
//    race = "lmao"
//};

//var updatedClient = await clientService.UpdateClientAsync(updateClient, newClient.id);
//Console.WriteLine($"New client updated: {updatedClient}");

//var clientWasDeleted = await clientService.DeleteClientAsync(newClient.id);
//if (clientWasDeleted == true) {
//    Console.WriteLine($"Client: {newClient.name} was deleted");
//}

//var llama = new Llama();
//await llama.UpdateModel();
//await llama.Chat();