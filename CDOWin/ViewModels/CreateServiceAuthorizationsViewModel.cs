using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateServiceAuthorizationsViewModel(IServiceAuthorizationService service, Client client) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service = service;

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
        Debug.WriteLine("didchange");
        if (string.IsNullOrWhiteSpace(Id)
            || string.IsNullOrWhiteSpace(Description)
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
            Id = Id,
            ClientID = Client.Id,
            CounselorrID = Client.CounselorID,
            Description = Description,
            StartDate = StartDate,
            EndDate = EndDate,
            Office = Office,
            UnitCost = UnitCost,
            UnitOfMeasurement = UnitOfMeasurement
        };

        await _service.CreateServiceAuthorizationAsync(sa);
    }
}
