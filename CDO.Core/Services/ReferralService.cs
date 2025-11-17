using CDO.Core.Constants;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class ReferralService : IReferralService {
    private readonly INetworkService _network;
    public List<Referral> Referrals { get; private set; } = new();

    public ReferralService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Referral>>(Endpoints.Referrals);
        if (data != null) {
            Referrals = data;
        }
    }

    public Task<List<Referral>?> GetAllReferralsAsync() {
        return _network.GetAsync<List<Referral>>(Endpoints.Referrals);
    }
}
