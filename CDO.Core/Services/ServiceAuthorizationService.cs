using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;

namespace CDO.Core.Services;

public class ServiceAuthorizationService : IServiceAuthorizationService {
    private readonly INetworkService _network;
    public List<ServiceAuthorization> ServiceAuthorizations { get; private set; } = new();

    public ServiceAuthorizationService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<ServiceAuthorization>?> GetAllServiceAuthorizationsAsync() {
        return _network.GetAsync<List<ServiceAuthorization>>(Endpoints.ServiceAuthorizations);
    }

    public Task<ServiceAuthorization?> GetServiceAuthorizationAsync(string id) {
        return _network.GetAsync<ServiceAuthorization>(Endpoints.ServiceAuthorization(id));
    }

    // -----------------------------
    // POST Methods
    // -----------------------------
    public Task<ServiceAuthorization?> CreateServiceAuthorizationAsync(NewServiceAuthorizationDTO dto) {
        return _network.PostAsync<NewServiceAuthorizationDTO, ServiceAuthorization>(Endpoints.ServiceAuthorizations, dto);
    }

    // -----------------------------
    // PATCH Methods
    // -----------------------------
    public Task<ServiceAuthorization?> UpdateServiceAuthorizationAsync(string id, UpdateServiceAuthorizationDTO dto) {
        return _network.UpdateAsync<UpdateServiceAuthorizationDTO, ServiceAuthorization>(Endpoints.ServiceAuthorization(id), dto);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteServiceAuthorizationAsync(string id) {
        return _network.DeleteAsync(Endpoints.ServiceAuthorization(id));
    }

}
