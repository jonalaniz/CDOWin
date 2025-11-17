using CDO.Core.Models;

namespace CDO.Core.Services {
    public interface IStateService {
        public Task InitializeAsync();
        public Task<List<State>?> GetAllStatesAsync();
    }
}
