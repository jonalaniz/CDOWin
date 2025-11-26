using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class ReferralService : IReferralService {
    private readonly INetworkService _network;
    public List<Referral> Referrals { get; private set; } = new();

    public ReferralService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Referral>>(Endpoints.Referrals);
        if (data != null) {
            Referrals = data;
        }
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Referral>?> GetAllReferralsAsync() {
        return _network.GetAsync<List<Referral>>(Endpoints.Referrals);
    }

    public Task<Referral?> GetReferralAsync(string id) {
        return _network.GetAsync<Referral>(Endpoints.Referral(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Referral?> CreateReferralAsync(ReferralDTO dto) {
        return _network.PostAsync<ReferralDTO, Referral>(Endpoints.Referrals, dto);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Referral?> UpdateReferralAsync(ReferralDTO dto, string id) {
        return _network.UpdateAsync<ReferralDTO, Referral>(Endpoints.Referral(id), dto);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteReferralAsync(string id) {
        return _network.DeleteAsync(Endpoints.Referral(id));
    }

}
