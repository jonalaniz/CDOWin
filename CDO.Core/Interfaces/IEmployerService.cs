using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces {
    public interface IEmployerService {

        // -----------------------------
        // GET Methods
        // -----------------------------
        public Task<List<Employer>?> GetAllEmployersAsync();

        public Task<Employer?> GetEmployerAsync(int id);

        // -----------------------------
        // POST Methods
        // -----------------------------
        //public Task<Employer?> CreateEmployerAsync(EmployerDTO dto);
        public Task<Result<Employer>> CreateEmployerAsync(EmployerDTO dto);

        // -----------------------------
        // PATCH Methods
        // -----------------------------
        public Task<Result<Employer>> UpdateEmployerAsync(int id, EmployerDTO dto);

        // -----------------------------
        // DELETE Methods
        // -----------------------------
        public Task<Result<bool>> DeleteEmployerAsync(int id);
    }
}
