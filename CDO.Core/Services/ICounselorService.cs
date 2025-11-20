using CDO.Core.Models;

namespace CDO.Core.Services {
    public interface ICounselorService {
        public Task InitializeAsync();
        public Task<List<Counselor>?> GetAllCounselorsAsync();

        public Task<Counselor?> GetCounselorAsync(int id);
    }
}
