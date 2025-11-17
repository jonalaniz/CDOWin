using CDO.Core.Models;

namespace CDO.Core.Services;

public interface IPOService {
    public Task InitializeAsync();
    public Task<List<PO>?> GetAllPOsAsync();
}
