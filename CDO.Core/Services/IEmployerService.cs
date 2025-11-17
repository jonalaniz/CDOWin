using CDO.Core.Models;

namespace CDO.Core.Services {
    public interface IEmployerService {
        public Task InitializeAsync();
        public Task<List<Employer>?> GetAllEmployersAsync();
    }
}
