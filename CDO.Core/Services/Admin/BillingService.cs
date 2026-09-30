using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Placements;
using CDO.Core.Interfaces;

namespace CDO.Core.Services.Admin;

public class BillingService(INetworkService network) {
    private readonly INetworkService _network = network;

    // -----------------------------
    // GET Methods
    // -----------------------------

    public Task<List<AdminSASummary>?> GetExpiringSAsAsync() {
        return _network.GetAsync<List<AdminSASummary>>(Endpoints.BillingExpiringSAs);
    }

    public Task<List<AdminSASummary>?> GetRecentSAsAsync() {
        return _network.GetAsync<List<AdminSASummary>>(Endpoints.BillingRecentSAs);
    }

    public Task<List<AdminSASummary>?> GetUnbilledSAsAsync() {
        return _network.GetAsync<List<AdminSASummary>>(Endpoints.BillingSAs);
    }

    public Task<List<PlacementSummary>?> GetNewPlacements() {
        return _network.GetAsync<List<PlacementSummary>>(Endpoints.BillingNewPlacements);
    }
}
