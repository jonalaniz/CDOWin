using CDO.Core.Constants;
using CDO.Core.DTOs.Counselors;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class CounselorService : ICounselorService {
    private readonly INetworkService _network;
    public List<CounselorSummary> Counselors { get; private set; } = new();

    public CounselorService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<CounselorSummary>?> GetAllCounselorSummariesAsync() {
        return _network.GetAsync<List<CounselorSummary>>(Endpoints.CounselorSummaries);
    }
    public Task<List<Counselor>?> GetAllCounselorsAsync() {
        return _network.GetAsync<List<Counselor>>(Endpoints.Counselors);
    }

    public Task<CounselorDetail?> GetCounselorAsync(int id) {
        return _network.GetAsync<CounselorDetail>(Endpoints.Counselor(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public async Task<Result> CreateCounselorAsync(NewCounselor dto) {
        var result = await _network.PostAsync(Endpoints.Counselors, dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result> UpdateCounselorAsync(int id, CounselorUpdate dto) {
        var result = await _network.UpdateAsync(Endpoints.Counselor(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteCounselorAsync(int id) {
        return _network.DeleteAsync(Endpoints.Counselor(id));
    }

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError TranslateError(AppError error) =>
        error.Kind switch {
            ErrorKind.Conflict => error with { Message = "A Counselor with this ID already exists." },
            ErrorKind.Validation => error with { Message = "Invalid data." },
            _ => error
        };
}
