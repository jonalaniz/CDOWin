using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateServiceAuthorizationsViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service;
    private readonly Client _client;

    private readonly int _clientID;

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial DateTime StartDate { get; set; } = DateTime.Now.Date;

    [ObservableProperty]
    public partial DateTime EndDate { get; set; } = DateTime.Now.Date;

    [ObservableProperty]
    public partial string? Office { get; set; }

    [ObservableProperty]
    public partial double? UnitCost { get; set; }

    [ObservableProperty]
    public partial string? UnitOfMeasurement { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    // =========================
    // Constructor
    // =========================
    public CreateServiceAuthorizationsViewModel(IServiceAuthorizationService service, Client client) {
        _service = service;
        _client = client;
        _clientID = client.id;
    }

    private bool CanSaveMethod() {
        if (string.IsNullOrWhiteSpace(Description)
            || string.IsNullOrWhiteSpace(UnitOfMeasurement)
            || UnitCost == null)
            return false;

        return true;
    }

    // =========================
    // Property Change Methods
    // =========================

    // =========================
    // CRUD Methods
    // =========================
    public async Task CreateSAAsync() {
        var sa = new CreateSADTO {
            clientID = _clientID,
            employerID = _client.counselorID,
            description = Description,
            startDate = StartDate,
            endDate = EndDate,
            office = Office,
            unitCost = UnitCost,
            unitOfMeasurement = UnitOfMeasurement
        };

        await _service.CreateServiceAuthorizationAsync(sa);
    }
}
