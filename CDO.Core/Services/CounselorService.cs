using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class CounselorService : ICounselorService {
    private readonly INetworkService _network;
    public List<Counselor> Counselors { get; private set; } = new();

    public CounselorService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // Service Initialization Tasks
    // -----------------------------
    public async Task InitializeAsync() {
        var data = await _network.GetAsync<List<Counselor>>(Endpoints.Counselors
        );
        if (data != null) {
            Counselors = data;
        }
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Counselor>?> GetAllCounselorsAsync() {
        return _network.GetAsync<List<Counselor>>(Endpoints.Counselors);
    }

    public Task<Counselor?> GetCounselorAsync(int id) {
        return _network.GetAsync<Counselor>(Endpoints.Counselor(id));
    }
    
    // -----------------------------
    // POST Methods
    // -----------------------------
        
    // -----------------------------
    // PATCH Methods
    // -----------------------------
        
    // -----------------------------
    // DELETE Methods
    // -----------------------------
}
