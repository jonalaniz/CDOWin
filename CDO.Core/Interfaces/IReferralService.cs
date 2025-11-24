using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IReferralService {

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public Task InitializeAsync();

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<Referral>?> GetAllReferralsAsync();

    public Task<Referral?> GetReferralAsync(string id);

    // -----------------------------
    // POST Methods
    // -----------------------------

    // -----------------------------
    // PATCH Methods
    // -----------------------------

    // -----------------------------
    // DELETE Methods
    // -----------------------------
}
