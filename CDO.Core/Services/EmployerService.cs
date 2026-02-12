using CDO.Core.Constants;
using CDO.Core.DTOs.Employers;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class EmployerService : IEmployerService {
    private readonly INetworkService _network;
    public List<Employer> Employers { get; private set; } = new();

    public EmployerService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Employer>?> GetAllEmployersAsync() {
        return _network.GetAsync<List<Employer>>(Endpoints.Employers);
    }

    public Task<List<EmployerSummary>?> GetAllEmployerSummariesAsync() {
        return _network.GetAsync<List<EmployerSummary>>(Endpoints.EmployerSummaries);
    }

    public Task<Employer?> GetEmployerAsync(int id) {
        return _network.GetAsync<Employer>(Endpoints.Employer(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public async Task<Result<Employer>> CreateEmployerAsync(EmployerDTO dto) {
        var result = await _network.PostAsync<EmployerDTO, Employer>(Endpoints.Employers, dto);
        if (!result.IsSuccess) return Result<Employer>.Fail(TranslateError(result.Error!));
        return Result<Employer>.Success(result.Value!);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public async Task<Result> UpdateEmployerAsync(int id, EmployerDTO dto) {
        var result = await _network.UpdateAsync(Endpoints.Employer(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteEmployerAsync(int id) {
        return _network.DeleteAsync(Endpoints.Employer(id));
    }

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError TranslateError(AppError error) =>
        error.Kind switch {
            ErrorKind.Conflict => error with { Message = "A Employer with this ID already exists." },
            ErrorKind.Validation => error with { Message = "Invalid data." },
            _ => error
        };
}
