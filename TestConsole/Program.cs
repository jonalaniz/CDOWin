using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");
Console.WriteLine($"Environment AKI-KEY: {apiKey}");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);
//network.Initialize("http://127.0.0.1:8080", "RISgMANlIwHwqLPvOTDs8ecmz37VyW8O");

IClientService clientService = new ClientService(network);
// IPOService POService = new POService(network);
//
// var POs = await POService.GetAllPOsAsync();
//
// foreach (var PO in POs) {
//     Console.WriteLine($"State ID: {PO.id}");
//     Console.WriteLine($"State Name: {PO.clientID}");
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

var clients = await clientService.GetAllClientsAsync();
var disabilityCount = 0;
var disability = "";
var criminalChargeCount = 0;
var criminalCharges = "";
var transportationCount = 0;
var transportation = "";
var conditionsCount = 0;
var conditions = "";
var employmnetGoalsCount = 0;
var employmentGoals = "";
var totalLength = 0;
var totalName = "";

foreach (var c in clients) {
    var length = 0;
    if (c.disability.Length > disabilityCount) {
        disabilityCount = c.disability.Length;
        disability = c.disability;
        length += c.disability.Length;
    }

    if (c.criminalCharge != null && c.criminalCharge.Length > criminalChargeCount) {
        criminalChargeCount = c.criminalCharge.Length;
        criminalCharges = c.criminalCharge;
        length += c.criminalCharge.Length;
    }

    if (c.transportation != null && c.transportation.Length > transportationCount) {
        transportationCount = c.transportation.Length;
        transportation = c.transportation;
        length += c.transportation.Length;
    }

    if (c.conditions != null && c.conditions.Length > conditionsCount) {
        conditionsCount = c.conditions.Length;
        conditions = c.conditions;
    }

    if (c.employmentGoal != null && c.employmentGoal.Length > employmnetGoalsCount) {
        employmnetGoalsCount = c.employmentGoal.Length;
        employmentGoals = c.employmentGoal;
    }

    if (length > totalLength) {
        totalLength = length;
        totalName = c.name;
    }
}

Console.WriteLine($"Longest disability: {disabilityCount}");
Console.WriteLine(employmentGoals);
Console.WriteLine($"Longest criminal charge: {criminalChargeCount}");
Console.WriteLine(criminalCharges);
Console.WriteLine($"Longest transportation: {transportationCount}");
Console.WriteLine(transportation);

Console.WriteLine($"Longest user: {totalName}");

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