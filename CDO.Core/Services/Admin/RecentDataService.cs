using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.Interfaces;

namespace CDO.Core.Services.Admin;

public class RecentDataService(INetworkService network) {
    private readonly INetworkService _network = network;

    // Returns a snapshot of work for the given day
    public Task<DailySnapshot?> GetSnapshot(string date) {
        var endpoint = Endpoints.DailySnapshot + $"?date={date}";
        return _network.GetAsync<DailySnapshot>(endpoint);
    }
}