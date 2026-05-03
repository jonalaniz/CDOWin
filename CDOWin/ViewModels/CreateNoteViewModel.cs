using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateNoteViewModel(IClientService service, int clientId) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IClientService _service = service;

    // =========================
    // Fields
    // =========================

    // Required
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string Note { get; set; }

    public DateTime Date = DateTime.UtcNow;

    public string Author = char.ToUpper(Environment.UserName[0]) + Environment.UserName[1..];

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    public bool CanSaveMethod() {
        if (string.IsNullOrWhiteSpace(Note)) {
            return false;
        }

        return true;
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task<Result<ClientNote>> CreateClientNoteAsync() {
        if (!CanSave)
            return Result<ClientNote>.Fail(new AppError(ErrorKind.Validation, "Missing required fields.", null));

        var note = new NewNote {
            Date = Date,
            Note = Note,
            Author = Author
        };

        return await _service.CreateClientNoteAsync(note, clientId);
    }
}