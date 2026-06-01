using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Users;
using CDO.Core.ErrorHandling;

namespace CDO.Core.Interfaces.Admin;

public interface IUserService {
    
// -----------------------------
// GET Methods
// -----------------------------
public Task<List<UserSummary>?> GetAllUsersSummariesAsync();

// -----------------------------
// POST Methods
// -----------------------------
public Task<Result<UserSummary>> CreateUserAsync(NewUser dto);

// -----------------------------
// PATCH Methods
// -----------------------------
public Task<Result> UpdateUserAsync(string id, UserUpdate dto);

// -----------------------------
// DELETE Methods
// -----------------------------
public Task<Result> DeleteUserAsync(string id);
}