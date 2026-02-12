using CDO.Core.DTOs.Employers;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces {
    public interface IEmployerService {

        // -----------------------------
        // GET Methods
        // -----------------------------
        public Task<List<EmployerSummary>?> GetAllEmployerSummariesAsync();
        public Task<List<Employer>?> GetAllEmployersAsync();

        public Task<Employer?> GetEmployerAsync(int id);

        // -----------------------------
        // POST Methods
        // -----------------------------
        public Task<Result> CreateEmployerAsync(EmployerDTO dto);

        // -----------------------------
        // PATCH Methods
        // -----------------------------
        public Task<Result> UpdateEmployerAsync(int id, EmployerDTO dto);

        // -----------------------------
        // DELETE Methods
        // -----------------------------
        public Task<Result> DeleteEmployerAsync(int id);
    }
}
