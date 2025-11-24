using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IPOService {

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public Task InitializeAsync();

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<PO>?> LoadPOsAsync();

    public Task<PO?> LoadPOAsync(string id);

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
