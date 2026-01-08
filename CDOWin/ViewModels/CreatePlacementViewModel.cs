using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreatePlacementViewModel(IPlacementService service, Client client, Employer employer) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IPlacementService _service = service;

    [ObservableProperty]
    public partial Client Client { get; set; } = client;

    [ObservableProperty]
    public partial Employer Employer { get; set; } = employer;

    [ObservableProperty]
    public partial int? PlacementNumber { get; set; }

    [ObservableProperty]
    public partial string? PoNumber { get; set; }

    [ObservableProperty]
    public partial string? Supervisor { get; set; }

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
    public partial string? ClientName { get; set; }

    [ObservableProperty]
    public partial string? CounselorName { get; set; }

    [ObservableProperty]
    public partial bool? Active { get; set; }

    [ObservableProperty]
    public partial string? Website { get; set; }

    [ObservableProperty]
    public partial string? DescriptionOfDuties { get; set; }

    [ObservableProperty]
    public partial string? NumbersOfHoursWorking { get; set; }

    [ObservableProperty]
    public partial string? FirstFiveDays1 { get; set; }

    [ObservableProperty]
    public partial string? FirstFiveDays2 { get; set; }

    [ObservableProperty]
    public partial string? FirstFiveDays3 { get; set; }

    [ObservableProperty]
    public partial string? FirstFiveDays4 { get; set; }

    [ObservableProperty]
    public partial string? FirstFiveDays5 { get; set; }

    [ObservableProperty]
    public partial string? DescriptionOfWorkSchedule { get; set; }

    [ObservableProperty]
    public partial string? HourlyOrMonthlyWages { get; set; }

    [ObservableProperty]
    public partial DateTime? HireDate { get; set; }

    [ObservableProperty]
    public partial DateTime? EndDate { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    public bool CanSaveMethod() {


        return true;
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task CreatePlacementAsync() {
        if (Client.EmployerID == null
            || Client.CounselorID == null)
            return;

        var placement = new PlacementDTO {
            PlacementNumber = PlacementNumber,
            EmployerID = Employer.Id.ToString(),
            ClientID = Client.Id,
            CounselorID = Client.CounselorID,
            PoNumber = PoNumber,
            Supervisor = Supervisor,
            SupervisorEmail = SupervisorEmail,
            SupervisorPhone = SupervisorPhone,
            Position = Position,
            Salary = Salary,
            DaysOnJob = DaysOnJob,
            ClientName = ClientName,
            CounselorName = CounselorName,
            Active = Active,
            Website = Website,
            DescriptionOfDuties = DescriptionOfDuties,
            NumbersOfHoursWorking = NumbersOfHoursWorking,
            FirstFiveDays1 = FirstFiveDays1,
            FirstFiveDays2 = FirstFiveDays2,
            FirstFiveDays3 = FirstFiveDays3,
            FirstFiveDays4 = FirstFiveDays4,
            FirstFiveDays5 = FirstFiveDays5,
            DescriptionOfWorkSchedule = DescriptionOfWorkSchedule,
            HourlyOrMonthlyWages = HourlyOrMonthlyWages,
            HireDate = HireDate,
            EndDate = EndDate
        };

        await _service.CreatePlacementAsync(placement);
    }
}
