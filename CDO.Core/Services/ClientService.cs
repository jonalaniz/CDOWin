using CDO.Core.Constants;
using CDO.Core.DTOs.Clients;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;

namespace CDO.Core.Services;

public class ClientService : IClientService {
    private readonly INetworkService _network;
    public List<ClientDetail> Clients { get; private set; } = new();

    public ClientService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<ClientDetail>?> GetAllClientsAsync() {
        return _network.GetAsync<List<ClientDetail>>(Endpoints.Clients);
    }

    public Task<List<ClientSummary>?> GetAllClientSummariesAsync() {
        return _network.GetAsync<List<ClientSummary>>(Endpoints.ClientSummaries);
    }

    public Task<ClientDetail?> GetClientAsync(int id) {
        return _network.GetAsync<ClientDetail>(Endpoints.Client(id));
    }

    // -----------------------------
    // POST
    // -----------------------------
    public async Task<Result> CreateClientAsync(NewClient dto) {
        var result = await _network.PostAsync(Endpoints.Clients, dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // PATCH
    // -----------------------------
    public async Task<Result> UpdateClientAsync(int id, ClientUpdate dto) {
        var result = await _network.UpdateAsync(Endpoints.Client(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteClientAsync(int id) {
        return _network.DeleteAsync(Endpoints.Client(id));
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------

    // -----------------------------
    // Utility Methods
    // -----------------------------
    private static AppError TranslateError(AppError error) =>
        error.Kind switch {
            ErrorKind.Conflict => error with { Message = "A client with this ID already exists." },
            ErrorKind.Validation => error with { Message = "Invalid data." },
            _ => error
        };
}
