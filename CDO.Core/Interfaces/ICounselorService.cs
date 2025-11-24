using CDO.Core.DTOs;
using CDO.Core.Models;

namespace CDO.Core.Interfaces {
    public interface ICounselorService {

        // -----------------------------
        // Service Initialization Tasks
        // -----------------------------
        public Task InitializeAsync();

        // -----------------------------
        // GET Methods
        // -----------------------------
        public Task<List<Counselor>?> GetAllCounselorsAsync();

        public Task<Counselor?> GetCounselorAsync(int id);

        // -----------------------------
        // POST Methods
        // -----------------------------
        public Task<Counselor?> CreateCounselorAsync(CreateCounselorDTO dto);

        // -----------------------------
        // PATCH Methods
        // -----------------------------
        public Task<Counselor?> UpdateCounselorAsync(UpdateCounselorDTO dto, int id);

        // -----------------------------
        // DELETE Methods
        // -----------------------------
        public Task<bool> DeleteCounselorAsync(int id);
    }
}
