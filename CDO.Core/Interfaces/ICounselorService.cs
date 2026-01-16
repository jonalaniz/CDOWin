using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces {
    public interface ICounselorService {

        // -----------------------------
        // GET Methods
        // -----------------------------
        public Task<List<Counselor>?> GetAllCounselorsAsync();

        public Task<Counselor?> GetCounselorAsync(int id);

        // -----------------------------
        // POST Methods
        // -----------------------------
        //public Task<Counselor?> CreateCounselorAsync(CreateCounselorDTO dto);
        public Task<Result<Counselor>> CreateCounselorAsync(CreateCounselorDTO dto);

        // -----------------------------
        // PATCH Methods
        // -----------------------------
        public Task<Result<Counselor>> UpdateCounselorAsync(int id, UpdateCounselorDTO dto);

        // -----------------------------
        // DELETE Methods
        // -----------------------------
        public Task<Result<bool>> DeleteCounselorAsync(int id);
    }
}
