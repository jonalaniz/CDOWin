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
    //public Task<State?> CreateStateAsync(CreateStateDTO dto);
    public Task<Result<State>> CreateStateAsync(CreateStateDTO dto);

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Result<State>> UpdateStateAsync(int id, UpdateStateDTO dto);

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteStateAsync(int id);

}
