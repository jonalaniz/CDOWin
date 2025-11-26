using CDO.Core.Constants;
using CDO.Core.DTOs;
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
    // Service Initialization Tasks
    // -----------------------------
    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Employer>>(Endpoints.Employers);
        if (data != null) {
            Employers = data;
        }
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Employer>?> GetAllEmployersAsync() {
        return _network.GetAsync<List<Employer>>(Endpoints.Employers);
    }

    public Task<Employer?> GetEmployerAsync(int id) {
        return _network.GetAsync<Employer>(Endpoints.Employer(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<Employer?> CreateEmployerAsync(EmployerDTO dto) {
        return _network.PostAsync<EmployerDTO, Employer>(Endpoints.Employers, dto);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<Employer?> UpdateEmployerAsync(EmployerDTO dto, int id) {
        return _network.UpdateAsync<EmployerDTO, Employer>(Endpoints.Employer(id), dto);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteEmployerAsync(int id) {
        return _network.DeleteAsync(Endpoints.Employer(id));
    }
}
