using CDO.Core.Constants;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Users;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Interfaces.Admin;

namespace CDO.Core.Services.Admin;

public class UserService : IUserService {
    private readonly INetworkService _network;
    public List<UserSummary> Users { get; private set; } = new();

    public UserService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<UserSummary>?> GetAllUsersSummariesAsync() {
        return _network.GetAsync<List<UserSummary>>(Endpoints.Users);
    }

    // -----------------------------
    // POST
    // -----------------------------
    public async Task<Result<UserSummary>> CreateUserAsync(NewUser dto) {
        var result = await _network.PostAsync<NewUser, UserSummary>(Endpoints.Users, dto);
        if (!result.IsSuccess) return Result<UserSummary>.Fail(TranslateError(result.Error!));
        return Result<UserSummary>.Success(result.Value!);
    }

    // -----------------------------
    // PATCH
    // -----------------------------
    public async Task<Result> UpdateUserAsync(string id, UserUpdate dto) {
        var result = await _network.UpdateAsync(Endpoints.User(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteUserAsync(string id) {
        return _network.DeleteAsync(Endpoints.User(id));
    }

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError TranslateError(AppError error) =>
        error.Kind switch {
            ErrorKind.Conflict => error with { Message = "A user with this ID already exists." },
            ErrorKind.Validation => error with { Message = "Invalid data." },
            _ => error
        };
}