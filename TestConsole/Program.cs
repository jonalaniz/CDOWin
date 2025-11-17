using CDO.Core.Services;

// Get environment variables
var apiKey = Environment.GetEnvironmentVariable("CDO_API_KEY");

var network = new NetworkService();
network.Initialize("https://api.jonalaniz.com", apiKey);

IReferralService ReferralService = new ReferralService(network);

var Referrals = await ReferralService.GetAllReferralsAsync();

foreach (var Referral in Referrals) {
    Console.WriteLine($"State ID: {Referral.id}");
    Console.WriteLine($"State Name: {Referral.clientID}");
}

//var llama = new Llama();

//await llama.UpdateModel();

//await llama.Chat();