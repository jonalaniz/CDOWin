using CDO.Core.DTOs;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IStateService {

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public Task InitializeAsync();

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<State>?> GetAllStatesAsync();

    public Task<State?> GetStateAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<State?> CreateStateAsync(CreateStateDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<State?> UpdateStateAsync(UpdateStateDTO dto, int id);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteStateAsync(int id);

}
