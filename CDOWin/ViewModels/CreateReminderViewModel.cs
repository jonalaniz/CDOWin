using CDO.Core.DTOs;
using CDO.Core.Interfaces;
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
    public partial string Description { get; set; } = string.Empty;

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => !string.IsNullOrWhiteSpace(Description);

    // =========================
    // Property Change Methods
    // =========================
    partial void OnDescriptionChanged(string value) {
        OnPropertyChanged(nameof(CanSave));
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task CreateReminderAsync() {
        var reminder = new CreateReminderDTO {
            ClientID = _clientId,
            Date = Date.ToUniversalTime(),
            Description = Description
        };

        await _service.CreateReminderAsync(reminder);
    }

}
