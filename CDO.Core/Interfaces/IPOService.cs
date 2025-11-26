using CDO.Core.DTOs;
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
    public Task<List<PO>?> GetAllPOsAsync();

    public Task<PO?> GetPOAsync(string id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<PO?> CreatePOAsync(NewPODTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<PO?> UpdatePOAsync(UpdatePODTO dto, string id);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeletePOAsync(string id);

}
