using CDO.Core.DTOs.Counselors;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces {
    public interface ICounselorService {

        // -----------------------------
        // GET Methods
        // -----------------------------
        public Task<List<CounselorSummary>?> GetAllCounselorSummariesAsync();
        public Task<List<Counselor>?> GetAllCounselorsAsync();

        public Task<CounselorDetail?> GetCounselorAsync(int id);

        // -----------------------------
        // POST Methods
        // -----------------------------
        public Task<Result<Counselor>> CreateCounselorAsync(NewCounselor dto);

        // -----------------------------
        // PATCH Methods
        // -----------------------------
        public Task<Result> UpdateCounselorAsync(int id, CounselorUpdate dto);

        // -----------------------------
        // DELETE Methods
        // -----------------------------
        public Task<Result> DeleteCounselorAsync(int id);
    }
}
