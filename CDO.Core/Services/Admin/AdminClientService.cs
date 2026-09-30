using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.Interfaces;

namespace CDO.Core.Services.Admin;

public class AdminClientService(INetworkService network) {
    private readonly INetworkService _network = network;

    // =============================
    // GET Methods (Cached)
    // =============================

    /// <summary>
    /// Returns all clients not updated in the past week.
    /// </summary>
    /// <returns>A list of AdminClientSummaries</returns>
    public Task<List<AdminClientSummary>?> GetStaleClientsAsync() {
        return _network.GetAsync<List<AdminClientSummary>>(Endpoints.AdminStaleClients);
    }

    public Task<List<AdminClientSummary>?> GetAllClientSummariesAsync() {
        return _network.GetAsync<List<AdminClientSummary>>(Endpoints.AdminAllClientSummaries);
    }

    // =============================
    // GET — Audit / Export (no cache)
    // =============================

    /// <summary>
    /// Returns all clients in database for audit/export purposes.
    /// </summary>
    /// <returns>A list of AdminClientDetail</returns>
    public Task<List<AdminClientDetail>?> GetAllClientRecordsAsync() {
        return _network.GetAsync<List<AdminClientDetail>>(Endpoints.AdminClientExport);
    }

    public Task<ClientHistory?> GetClientHistory(int id) {
        return _network.GetAsync<ClientHistory>(Endpoints.ClientHistory(id));
    }

}
