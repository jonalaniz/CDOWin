using CDO.Core.Models;

namespace CDO.Core.Services;

public interface IPoService {
    public Task InitializeAsync();
    public Task<List<Pos>?> GetAllPosAsync();
}
