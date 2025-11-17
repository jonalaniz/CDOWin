using CDO.Core.Constants;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class EmployerService : IEmployerService {
    private readonly INetworkService _network;
    public List<Employer> Employers { get; private set; } = new();

    public EmployerService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Employer>>(Endpoints.Employers);
        if (data != null) {
            Employers = data;
        }
    }

    public Task<List<Employer>?> GetAllEmployersAsync() {
        return _network.GetAsync<List<Employer>>(Endpoints.Employers);
    }
}
