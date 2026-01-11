using CDO.Core.Constants;
using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using System.Runtime.InteropServices;

namespace CDO.Core.Services;

public class ClientService : IClientService {
    private readonly INetworkService _network;
    public List<Client> Clients { get; private set; } = new();

    public ClientService(INetworkService network) {
        _network = network;
    }

    // -----------------------------
    // GET
    // -----------------------------
    public Task<List<Client>?> GetAllClientsAsync() {
        return _network.GetAsync<List<Client>>(Endpoints.Clients);
    }

    public Task<List<ClientSummaryDTO>?> GetAllClientSummariesAsync() {
        return _network.GetAsync<List<ClientSummaryDTO>>(Endpoints.ClientSummaries);
    }

    public Task<Client?> GetClientAsync(int id) {
        return _network.GetAsync<Client>(Endpoints.Client(id));
    }

    // -----------------------------
    // POST
    // -----------------------------
    public async Task<Result<Client>> CreateClientAsync(CreateClientDTO dto) {
        var result = await _network.PostAsync<CreateClientDTO, Client>(Endpoints.Clients, dto);
        if (!result.IsSuccess) return Result<Client>.Fail(TranslateError(result.Error!));
        return Result<Client>.Success(result.Value!);
    }

    // -----------------------------
    // PATCH
    // -----------------------------
    public async Task<Result<Client>> UpdateClientAsync(int id, UpdateClientDTO dto) {
        var result = await _network.UpdateAsync<UpdateClientDTO, Client>(Endpoints.Client(id), dto);
        if (!result.IsSuccess) return Result<Client>.Fail(TranslateError(result.Error!));
        return Result<Client>.Success(result.Value!);
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<bool> DeleteClientAsync(int id) {
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
