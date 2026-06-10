using CDO.Core.Constants;
using CDO.Core.DTOs.Placements;
using CDO.Core.DTOs.SAs;
using CDO.Core.Interfaces;

namespace CDO.Core.Services.Admin;

public class BillingService {
    private readonly INetworkService _network;

    public BillingService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET Methods
    // -----------------------------

    public Task<List<SASummary>?> GetExpiringSAsAsync() {
        return _network.GetAsync<List<SASummary>>(Endpoints.BillingExpiringSAs);
    } 

    public Task<List<SASummary>?> GetUnbilledSAsAsync() {
        return _network.GetAsync<List<SASummary>>(Endpoints.BillingSAs);
    }

    public Task<List<PlacementSummary>?> GetUnbilledPlacementsAsync() {
        return _network.GetAsync<List<PlacementSummary>>(Endpoints.BillingPlacements);
    }
}
