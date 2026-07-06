using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IStateService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<State>?> GetAllStatesAsync();
}
