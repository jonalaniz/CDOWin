using CDO.Core.DTOs;
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
    public Task<Referral?> CreateReferralAsync(ReferralDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Referral?> UpdateReferralAsync(ReferralDTO dto, string id);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteReferralAsync(string id);

}
