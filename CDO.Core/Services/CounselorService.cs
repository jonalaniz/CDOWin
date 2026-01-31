using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class CounselorService : ICounselorService {
    private readonly INetworkService _network;
    public List<CounselorSummaryDTO> Counselors { get; private set; } = new();

    public CounselorService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Counselor>?> GetAllCounselorsAsync() {
        return _network.GetAsync<List<Counselor>>(Endpoints.Counselors);
    }

    public Task<List<CounselorSummaryDTO>?> GetAllCounselorSummariesAsync() {
        return _network.GetAsync<List<CounselorSummaryDTO>>(Endpoints.CounselorSummaries);
    }

    public Task<CounselorResponseDTO?> GetCounselorAsync(int id) {
        return _network.GetAsync<CounselorResponseDTO>(Endpoints.Counselor(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public async Task<Result<Counselor>> CreateCounselorAsync(CreateCounselorDTO dto) {
        var result = await _network.PostAsync<CreateCounselorDTO, Counselor>(Endpoints.Counselors, dto);
        if (!result.IsSuccess) return Result<Counselor>.Fail(TranslateError(result.Error!));
        return Result<Counselor>.Success(result.Value!);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result<Counselor>> UpdateCounselorAsync(int id, UpdateCounselorDTO dto) {
        var result = await _network.UpdateAsync<UpdateCounselorDTO, Counselor>(Endpoints.Counselor(id), dto);
        if (!result.IsSuccess) return Result<Counselor>.Fail(TranslateError(result.Error!));
        return Result<Counselor>.Success(result.Value!);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result<bool>> DeleteCounselorAsync(int id) {
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
