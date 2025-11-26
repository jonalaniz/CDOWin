using CDO.Core.DTOs;
using CDO.Core.Models;

namespace CDO.Core.Interfaces {
    public interface IEmployerService {

        // -----------------------------
        // Service Initialization Tasks
        // -----------------------------
        public Task InitializeAsync();

        // -----------------------------
        // GET Methods
        // -----------------------------
        public Task<List<Employer>?> GetAllEmployersAsync();

        public Task<Employer?> GetEmployerAsync(int id);

        // -----------------------------
        // POST Methods
        // -----------------------------
        public Task<Employer?> CreateEmployerAsync(EmployerDTO dto);
        
        // -----------------------------
        // PATCH Methods
        // -----------------------------
        public Task<Employer?> UpdateEmployerAsync(EmployerDTO dto, int id);

        // -----------------------------
        // DELETE Methods
        // -----------------------------
        public Task<bool> DeleteEmployerAsync(int id);
    }
}
