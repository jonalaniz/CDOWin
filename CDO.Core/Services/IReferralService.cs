using CDO.Core.Models;

namespace CDO.Core.Services;

public interface IReferralService {
    public Task InitializeAsync();
    public Task<List<Referral>?> GetAllReferralsAsync();
}
