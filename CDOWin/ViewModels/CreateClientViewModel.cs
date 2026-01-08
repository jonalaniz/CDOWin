using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateClientViewModel(IClientService service) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IClientService _service = service;

    // Required Fields

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? FirstName { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? LastName { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? Counselor { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? City { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? State { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? Disability { get; set; }

    // Optional Fields

    [ObservableProperty]
    public partial int? Ssn { get; set; }

    [ObservableProperty]
    public partial string? CaseID { get; set; }

    [ObservableProperty]
    public partial string? Address1 { get; set; }

    [ObservableProperty]
    public partial string? Address2 { get; set; }

    [ObservableProperty]
    public partial string? Zip { get; set; }

    [ObservableProperty]
    public partial DateTime? Dob { get; set; }

    [ObservableProperty]
    public partial DateTime? StartDate { get; set; }

    [ObservableProperty]
    public partial string? DriversLicense { get; set; }

    [ObservableProperty]
    public partial string? Phone1 { get; set; }

    [ObservableProperty]
    public partial string? Phone1Identity { get; set; }

    [ObservableProperty]
    public partial string? Phone2 { get; set; }

    [ObservableProperty]
    public partial string? Phone2Identity { get; set; }

    [ObservableProperty]
    public partial string? Phone3 { get; set; }

    [ObservableProperty]
    public partial string? Phone3Identity { get; set; }

    [ObservableProperty]
    public partial string? Email { get; set; }

    [ObservableProperty]
    public partial string? EmailIdentity { get; set; }

    [ObservableProperty]
    public partial string? Email2 { get; set; }

    [ObservableProperty]
    public partial string? Email2Identity { get; set; }

    [ObservableProperty]
    public partial int? CounselorID { get; set; }

    [ObservableProperty]
    public partial string? CounselorEmail { get; set; }

    [ObservableProperty]
    public partial string? CounselorPhone { get; set; }

    [ObservableProperty]
    public partial string? CounselorFax { get; set; }

    [ObservableProperty]
    public partial string? ClientNotes { get; set; }

    [ObservableProperty]
    public partial string? Conditions { get; set; }

    [ObservableProperty]
    public partial string? DocumentFolder { get; set; }

    [ObservableProperty]
    public partial bool? Active { get; set; }

    [ObservableProperty]
    public partial string? EmploymentGoal { get; set; }

    [ObservableProperty]
    public partial int? EmployerID { get; set; }

    [ObservableProperty]
    public partial string? Status { get; set; }

    [ObservableProperty]
    public partial string? Benefit { get; set; }

    [ObservableProperty]
    public partial string? CriminalCharge { get; set; }

    [ObservableProperty]
    public partial string? Education { get; set; }

    [ObservableProperty]
    public partial string? Transportation { get; set; }

    [ObservableProperty]
    public partial bool? ResumeRequired { get; set; }

    [ObservableProperty]
    public partial bool? ResumeCompleted { get; set; }

    [ObservableProperty]
    public partial bool? VideoInterviewRequired { get; set; }

    [ObservableProperty]
    public partial bool? VideoInterviewCompleted { get; set; }

    [ObservableProperty]
    public partial bool? ReleasesCompleted { get; set; }

    [ObservableProperty]
    public partial bool? OrientationCompleted { get; set; }

    [ObservableProperty]
    public partial bool? DataSheetCompleted { get; set; }

    [ObservableProperty]
    public partial bool? ElevatorSpeechCompleted { get; set; }

    [ObservableProperty]
    public partial string? Race { get; set; }

    [ObservableProperty]
    public partial string? FluentLanguages { get; set; }

    [ObservableProperty]
    public partial string? Premium { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    public bool CanSaveMethod() {
        if (string.IsNullOrWhiteSpace(FirstName)
            || string.IsNullOrWhiteSpace(LastName)
            || string.IsNullOrWhiteSpace(Counselor)
            || string.IsNullOrWhiteSpace(City)
            || string.IsNullOrWhiteSpace(State)
            || string.IsNullOrWhiteSpace(Disability)) {
            return false;
        }

        return true;
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task CreateClientAsync() {
        if (FirstName == null
            || LastName == null
            || Counselor == null
            || City == null
            || State == null
            || Disability == null)
            return;

        var client = new CreateClientDTO {
            FirstName = FirstName,
            LastName = LastName,
            Counselor = Counselor,
            City = City,
            State = State,
            Disability = Disability,
            Ssn = Ssn,
            CaseID = CaseID,
            Address1 = Address1,
            Address2 = Address2,
            Zip = Zip,
            Dob = Dob,
            StartDate = StartDate,
            DriversLicense = DriversLicense,
            Phone1 = Phone1,
            Phone1Identity = Phone1Identity,
            Phone2 = Phone2,
            Phone2Identity = Phone2Identity,
            Phone3 = Phone3,
            Phone3Identity = Phone3Identity,
            Email = Email,
            EmailIdentity = EmailIdentity,
            Email2 = Email2,
            Email2Identity = Email2Identity,
            CounselorID = CounselorID,
            CounselorEmail = CounselorEmail,
            CounselorPhone = CounselorPhone,
            CounselorFax = CounselorFax,
            ClientNotes = ClientNotes,
            Conditions = Conditions,
            DocumentFolder = DocumentFolder,
            Active = Active,
            EmploymentGoal = EmploymentGoal,
            EmployerID = EmployerID,
            Status = Status,
            Benefit = Benefit,
            CriminalCharge = CriminalCharge,
            Education = Education,
            Transportation = Transportation,
            ResumeRequired = ResumeRequired,
            ResumeCompleted = ResumeCompleted,
            VideoInterviewRequired = VideoInterviewRequired,
            VideoInterviewCompleted = VideoInterviewCompleted,
            ReleasesCompleted = ReleasesCompleted,
            OrientationCompleted = OrientationCompleted,
            DataSheetCompleted = DataSheetCompleted,
            ElevatorSpeechCompleted = ElevatorSpeechCompleted,
            Race = Race,
            FluentLanguages = FluentLanguages,
            Premium = Premium
        };

        await _service.CreateClientAsync(client);
    }
}