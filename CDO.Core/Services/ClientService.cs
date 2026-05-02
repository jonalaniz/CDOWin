using CDO.Core.Constants;
using CDO.Core.DTOs.Clients;
using CDO.Core.DTOs.Clients.Notes;
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
        return _network.GetAsync<List<ClientSummary>>(Endpoints.Clients);
    }

    public Task<ClientDetail?> GetClientAsync(int id, CancellationToken ct = default) {
        return _network.GetAsync<ClientDetail>(Endpoints.Client(id), ct);
    }

    // -----------------------------
    // POST
    // -----------------------------
    public async Task<Result<ClientDetail>> CreateClientAsync(NewClient dto) {
        var result = await _network.PostAsync<NewClient, ClientDetail>(Endpoints.Clients, dto);
        if (!result.IsSuccess) return Result<ClientDetail>.Fail(TranslateError(result.Error!));
        return Result<ClientDetail>.Success(result.Value!);
    }

    public async Task<Result<ClientNote>> CreateClientNoteAsync(NewNote dto, int clientId) {
        var result = await _network.PostAsync<NewNote, ClientNote>(Endpoints.Note(clientId), dto);
        if (!result.IsSuccess) return Result<ClientNote>.Fail(TranslateError(result.Error!));
        return Result<ClientNote>.Success(result.Value!);
    }

    // -----------------------------
    // PATCH
    // -----------------------------
    public async Task<Result> UpdateClientAsync(int id, ClientUpdate dto) {
        var result = await _network.UpdateAsync(Endpoints.Client(id), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    public async Task<Result> UpdateClientNote(NoteUpdate dto, int clientId, int noteId) {
        var result = await _network.UpdateAsync(Endpoints.Note(clientId, noteId), dto);
        if (!result.IsSuccess) return Result.Fail(TranslateError(result.Error!));
        return Result.Success();
    }

    // -----------------------------
    // DELETE Methods
    // -----------------------------
    public Task<Result> DeleteClientAsync(int id) {
        return _network.DeleteAsync(Endpoints.Client(id));
    }

    public Task<Result> DeleteClientNoteAsync(int clientId, int noteId) {
        return _network.DeleteAsync(Endpoints.Note(clientId, noteId));
    }

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
