using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreatePlacementViewModel(IPlacementService service, DataInvalidationService dataInvalidationService, Client client) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IPlacementService _service = service;
    private readonly DataInvalidationService _invalidation = dataInvalidationService;

    // =========================
    // Fields
    // =========================

    // Placement Specific
    public string? Position { get; set; }
    public DateTime? HireDate { get; set; }
    public  DateTime? EndDate { get; set; }
    public float? DaysOnJob { get; set; }
    public string? Day1 { get; set; }
    public string? Day2 { get; set; }
    public string? Day3 { get; set; }
    public string? Day4 { get; set; }
    public string? Day5 { get; set; }
    public string? JobDuties { get; set; }
    public string? HoursWorking { get; set; }
    public string? WorkSchedule { get; set; }
    public string? Wages { get; set; }
    public string? Benefits { get; set; }

    // SA/Invoice Specific
    public int? InvoiceID { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? SaNumber { get; set; }

    // Client Secific
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial Client Client { get; set; } = client;

    // EmployerName Specific
    public int? EmployerID { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? EmployerName { get; set; }
    public string? EmployerPhone { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; } = "TX";
    public string? Zip { get; set; }
    public string? SupervisorName { get; set; }
    public string? SupervisorEmail { get; set; }
    public string? SupervisorPhone { get; set; }
    public string? Website { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    public bool CanSaveMethod() {
        if (EmployerName == null
            || Client == null
            || SaNumber == null)
            return false;

        return true;
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task<Result<PlacementDetail>> CreatePlacementAsync() {
        if (Client.CounselorID == null)
            return Result<PlacementDetail>.Fail(new AppError(ErrorKind.Validation, "Client is missing a Counselor, assign on and try again."));

        int placementsNumber = 1;
        if (Client.Placements?.Length is int length && length > 0)
            placementsNumber = length + 1;

        var placementEmployer = new PlacementEmployer {
            EmployerID = EmployerID,
            Name = EmployerName,
            Phone = EmployerPhone,
            Address1 = Address1,
            Address2 = Address2,
            City = City,
            State = State,
            Zip = Zip,
            SupervisorName = SupervisorName,
            SupervisorEmail = SupervisorEmail,
            SupervisorPhone = SupervisorPhone,
            Website = Website
        };

        var placement = new NewPlacement {
            // Placement Specific
            Active = Client.Active,
            PlacementNumber = placementsNumber,
            Position = Position,
            HireDate = HireDate,
            EndDate = EndDate,
            DaysOnJob = DaysOnJob,
            Day1 = Day1,
            Day2 = Day2,
            Day3 = Day3,
            Day4 = Day4,
            Day5 = Day5,
            JobDuties = JobDuties,
            HoursWorking = HoursWorking,
            WorkSchedule = WorkSchedule,
            Wages = Wages,
            Benefits = Benefits,

            // SA/Invoice Specific
            InvoiceID = InvoiceID,
            SaNumber = SaNumber,

            // Client Specific
            ClientID = Client.Id,
            ClientName = $"{Client.FirstName} {Client.LastName}",

            // Counselor Specific
            CounselorID = Client.CounselorID,
            CounselorName = Client.CounselorReference?.Name,

            // Employer Specific
            Employer = placementEmployer
        };

        _invalidation.InvalidatePlacements();
        return await _service.CreatePlacementAsync(placement);
    }
}
