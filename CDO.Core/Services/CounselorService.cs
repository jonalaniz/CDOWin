using CDO.Core.Constants;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class CounselorService : ICounselorService {
    private readonly INetworkService _network;
    public List<Counselor> Counselors { get; private set; } = new();

    public CounselorService(INetworkService network) {
        _network = network;
    }

    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Counselor>>("/api/counselors/");
        if (data != null) {
            Counselors = data;
        }
    }

    public Task<List<Counselor>?> GetAllCounselorsAsync() {
        return _network.GetAsync<List<Counselor>>("/api/counselors/");
    }

    public Task<Counselor?> GetCounselorAsync(int id) {
        return _network.GetAsync<Counselor>(Endpoints.Counselor(id));
    }
}
