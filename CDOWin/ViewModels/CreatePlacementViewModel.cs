using CDO.Core.DTOs;
using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Office.Interop.Word;
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
    [ObservableProperty]
    public partial string? Position { get; set; }

    [ObservableProperty]
    public partial DateTime? HireDate { get; set; }

    [ObservableProperty]
    public partial DateTime? EndDate { get; set; }

    [ObservableProperty]
    public partial float? DaysOnJob { get; set; }

    [ObservableProperty]
    public partial string? Day1 { get; set; }

    [ObservableProperty]
    public partial string? Day2 { get; set; }

    [ObservableProperty]
    public partial string? Day3 { get; set; }

    [ObservableProperty]
    public partial string? Day4 { get; set; }

    [ObservableProperty]
    public partial string? Day5 { get; set; }

    [ObservableProperty]
    public partial string? JobDuties { get; set; }

    [ObservableProperty]
    public partial string? HoursWorking { get; set; }

    [ObservableProperty]
    public partial string? WorkSchedule { get; set; }

    [ObservableProperty]
    public partial string? Wages { get; set; }

    [ObservableProperty]
    public partial string? Benefits { get; set; }

    // SA/Invoice Specific
    [ObservableProperty]
    public partial int? InvoiceID { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? SaNumber { get; set; }

    // Client Secific
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial Client Client { get; set; } = client;

    // EmployerName Specific
    [ObservableProperty]
    public partial int? EmployerID { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? EmployerName { get; set; }

    [ObservableProperty]
    public partial string? EmployerPhone { get; set; }

    [ObservableProperty]
    public partial string? Address1 { get; set; }

    [ObservableProperty]
    public partial string? Address2 { get; set; }

    [ObservableProperty]
    public partial string? City { get; set; }

    [ObservableProperty]
    public partial string? State { get; set; } = "TX";

    [ObservableProperty]
    public partial string? Zip { get; set; }

    [ObservableProperty]
    public partial string? SupervisorName { get; set; }

    [ObservableProperty]
    public partial string? SupervisorEmail { get; set; }

    [ObservableProperty]
    public partial string? SupervisorPhone { get; set; }

    [ObservableProperty]
    public partial string? Website { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    public bool CanSaveMethod() {
        Debug.WriteLine($"{EmployerID == null} {Client == null} {SaNumber == null}");
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
