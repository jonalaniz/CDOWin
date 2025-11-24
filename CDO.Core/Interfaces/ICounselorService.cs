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
        
        // -----------------------------
        // PATCH Methods
        // -----------------------------
        
        // -----------------------------
        // DELETE Methods
        // -----------------------------
    }
}
