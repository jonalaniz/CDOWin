using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateServiceAuthorizationsViewModel(IServiceAuthorizationService service, DataInvalidationService dataInvalidationService, Client client) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service = service;
    private readonly DataInvalidationService _invalidation = dataInvalidationService;

    // =========================
    // Fields
    // =========================
    [ObservableProperty]
    public partial Client Client { get; set; } = client;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string Id { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial DateTime StartDate { get; set; } = DateTime.Now.Date;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial DateTime EndDate { get; set; } = DateTime.Now.Date;

    [ObservableProperty]
    public partial string? Office { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial double? UnitCost { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? UnitOfMeasurement { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    private bool CanSaveMethod() {
        if (Client.CounselorID == null
            || string.IsNullOrWhiteSpace(Id)
            || string.IsNullOrWhiteSpace(Description)
            || string.IsNullOrWhiteSpace(UnitOfMeasurement)
            || UnitCost == null)
            return false;

        return true;
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task<Result<Invoice>> CreateSAAsync() {
        var sa = new CreateInvoiceDTO {
            ServiceAuthorizationNumber = Id,
            ClientID = Client.Id,
            CounselorrID = Client.CounselorID,
            Description = Description,
            StartDate = StartDate,
            EndDate = EndDate,
            Office = Office,
            UnitCost = UnitCost,
            UnitOfMeasurement = UnitOfMeasurement
        };

        _invalidation.InvalidateSAs();
        return await _service.CreateServiceAuthorizationAsync(sa);
    }
}
