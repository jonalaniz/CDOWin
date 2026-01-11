using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateReminderViewModel(IReminderService service, int clientId) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IReminderService _service = service;
    private readonly int _clientId = clientId;

    // =========================
    // Fields
    // =========================
    public DateTime Date = DateTime.Now.Date;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string Description { get; set; } = string.Empty;

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => !string.IsNullOrWhiteSpace(Description);

    // =========================
    // CRUD Methods
    // =========================
    public async Task<Result<Reminder>> CreateReminderAsync() {
        var reminder = new CreateReminderDTO {
            ClientID = _clientId,
            Date = Date.ToUniversalTime(),
            Description = Description
        };

        return await _service.CreateRemindersAsync(reminder);
    }
}
