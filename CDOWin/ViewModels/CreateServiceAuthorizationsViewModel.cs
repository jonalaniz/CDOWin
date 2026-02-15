using CDO.Core.DTOs.Clients;
using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDOWin.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateServiceAuthorizationsViewModel(IServiceAuthorizationService service, DataInvalidationService dataInvalidationService, ClientDetail client) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service = service;
    private readonly DataInvalidationService _invalidation = dataInvalidationService;

    // =========================
    // Fields
    // =========================
    public ClientDetail Client { get; set; } = client;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string SANumber { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial DateTime StartDate { get; set; } = DateTime.Now.Date;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial DateTime EndDate { get; set; } = DateTime.Now.Date;
    public string? Office { get; set; }

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
            || Client.CounselorReference == null
            || string.IsNullOrWhiteSpace(Client.CaseID)
            || string.IsNullOrWhiteSpace(SANumber)
            || string.IsNullOrWhiteSpace(Description)
            || string.IsNullOrWhiteSpace(UnitOfMeasurement)
            || UnitCost == null)
            return false;

        return true;
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task<Result<InvoiceDetail>> CreateSAAsync() {
        var invoice = new NewSA {
            ServiceAuthorizationNumber = SANumber,
            Office = Office,
            Description = Description,
            StartDate = StartDate,
            EndDate = EndDate,
            UnitCost = UnitCost,
            UnitOfMeasurement = UnitOfMeasurement,

            ClientID = Client.Id,
            ClientName = Client.FormattedName,
            CaseID = Client.CaseID!,

            CounselorID = Client.CounselorID,
            CounselorName = Client.CounselorReference!.Name!,
            SecretaryName = Client.CounselorReference.SecretaryName,
        };

        _invalidation.InvalidateSAs();
        return await _service.CreateServiceAuthorizationAsync(invoice);
    }
}
