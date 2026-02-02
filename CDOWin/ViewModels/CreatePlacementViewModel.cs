using CDO.Core.DTOs;
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
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial Client Client { get; set; } = client;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial int? EmployerID { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial int? PlacementNumber { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? SaNumber { get; set; }

    [ObservableProperty]
    public partial string? SupervisorName { get; set; }

    [ObservableProperty]
    public partial string? SupervisorEmail { get; set; }

    [ObservableProperty]
    public partial string? SupervisorPhone { get; set; }

    [ObservableProperty]
    public partial string? Position { get; set; }

    [ObservableProperty]
    public partial string? Salary { get; set; }

    [ObservableProperty]
    public partial float? DaysOnJob { get; set; }

    [ObservableProperty]
    public partial string? Website { get; set; }

    [ObservableProperty]
    public partial string? JobDuties { get; set; }

    [ObservableProperty]
    public partial string? HoursWorking { get; set; }

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
    public partial string? WorkSchedule { get; set; }

    [ObservableProperty]
    public partial string? Wages { get; set; }

    [ObservableProperty]
    public partial DateTime? HireDate { get; set; }

    [ObservableProperty]
    public partial DateTime? EndDate { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    public bool CanSaveMethod() {
        Debug.WriteLine($"{EmployerID == null} {Client == null} {PlacementNumber == null} {SaNumber == null}");
        if (EmployerID == null
            || Client == null
            || PlacementNumber == null
            || SaNumber == null)
            return false;

        return true;
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task<Result<Placement>> CreatePlacementAsync() {
        if (Client.CounselorID == null)
            return Result<Placement>.Fail(new AppError(ErrorKind.Validation, "Client is missing a Counselor, assign on and try again."));

        var placement = new PlacementDTO {
            PlacementNumber = PlacementNumber,
            EmployerID = EmployerID.ToString(),
            ClientID = Client.Id,
            CounselorID = Client.CounselorID,
            SaNumber = SaNumber,
            SupervisorName = SupervisorName,
            SupervisorEmail = SupervisorEmail,
            SupervisorPhone = SupervisorPhone,
            Position = Position,
            Salary = Salary,
            DaysOnJob = DaysOnJob,
            ClientName = Client.FormattedName,
            CounselorName = Client.CounselorReference?.Name,
            Active = true,
            Website = Website,
            JobDuties = JobDuties,
            HoursWorking = HoursWorking,
            Day1 = Day1,
            Day2 = Day2,
            Day3 = Day3,
            Day4 = Day4,
            Day5 = Day5,
            WorkSchedule = WorkSchedule,
            Wages = Wages,
            HireDate = HireDate,
            EndDate = EndDate
        };

        _invalidation.InvalidatePlacements();
        return await _service.CreatePlacementAsync(placement);
    }
}
