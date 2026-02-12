using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Models;

namespace CDO.Core.Interfaces;

public interface IStateService {

    // -----------------------------
    // GET Methods
    // -----------------------------
    public Task<List<State>?> GetAllStatesAsync();

    public Task<State?> GetStateAsync(int id);

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Result> CreateStateAsync(CreateStateDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result> UpdateStateAsync(int id, UpdateStateDTO dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteStateAsync(int id);

}
