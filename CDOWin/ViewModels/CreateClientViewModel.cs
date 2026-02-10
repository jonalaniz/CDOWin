using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateClientViewModel(IClientService service) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IClientService _service = service;


    // =========================
    // Fields
    // =========================

    // Required

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? FirstName { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? LastName { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial int? CounselorID { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? CounselorName { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? City { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? State { get; set; } = "TX";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string? Disability { get; set; }
    public bool TTW { get; set; } = false;

    // Optional Fields
    public int? Ssn { get; set; }
    public string? CaseID { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Zip { get; set; }
    public DateTime? Dob { get; set; }
    public DateTime? StartDate { get; set; }
    public string? DriversLicense { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone1Identity { get; set; }
    public string? Phone2 { get; set; }
    public string? Phone2Identity { get; set; }
    public string? Phone3 { get; set; }
    public string? Phone3Identity { get; set; }
    public string? Email { get; set; }
    public string? EmailIdentity { get; set; }
    public string? Email2 { get; set; }
    public string? Email2Identity { get; set; }
    public string? ClientNotes { get; set; }
    public string? Conditions { get; set; }
    public bool? Active { get; set; }
    public string? EmploymentGoal { get; set; }
    public int? EmployerID { get; set; }
    public string? Status { get; set; }
    public string? Benefit { get; set; }
    public string? CriminalCharge { get; set; }
    public string? Education { get; set; }
    public string? Transportation { get; set; }
    public bool? ResumeRequired { get; set; } = false;
    public bool? ResumeCompleted { get; set; } = false;
    public bool? VideoInterviewRequired { get; set; } = false;
    public bool? VideoInterviewCompleted { get; set; } = false;
    public bool? ReleasesCompleted { get; set; } = false;
    public bool? OrientationCompleted { get; set; } = false;
    public bool? DataSheetCompleted { get; set; } = false;
    public bool? ElevatorSpeechCompleted { get; set; } = false;
    public string? Race { get; set; }
    public string? FluentLanguages { get; set; }
    public string? Premium { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => CanSaveMethod();

    public bool CanSaveMethod() {
        if (string.IsNullOrWhiteSpace(FirstName)
            || string.IsNullOrWhiteSpace(LastName)
            || CounselorID == null
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
    public async Task<Result<Client>> CreateClientAsync() {
        if (FirstName == null
            || LastName == null
            || CounselorID == null
            || City == null
            || State == null
            || Disability == null)
            return Result<Client>.Fail(new AppError(ErrorKind.Validation, "Missing required fields.", null));

        // Create Document folder
        var folderName = $"Z:\\DARS Clients\\{CounselorName}-{FirstName} {LastName}";
        if (!CreateDocumentFolder(folderName)) return Result<Client>.Fail(new AppError(ErrorKind.Conflict, "Folder already exists.", null));

        var client = new CreateClientDTO {
            FirstName = FirstName,
            LastName = LastName,
            TTW = TTW,
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
            ClientNotes = ClientNotes,
            Conditions = Conditions,
            DocumentFolder = folderName,
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

        return await _service.CreateClientAsync(client);
    }

    private bool CreateDocumentFolder(string path) {
        if (!TryEnsureDirectory(path, out var error)) {
            Debug.WriteLine(error);
            return false;
        }

        return true;
    }

    public static bool TryEnsureDirectory(string path, out string? error) {
        error = null;

        try {
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "\\1850-Elevator Spch-resume");
            Directory.CreateDirectory(path + "\\Billing Reports");
            Directory.CreateDirectory(path + "\\BST-Release-Orientation");
            Directory.CreateDirectory(path + "\\IPE-Supporting Docs");
            Directory.CreateDirectory(path + "\\Plan 1845 A&B");
            Directory.CreateDirectory(path + "\\Referrals (VR5000)");
            Directory.CreateDirectory(path + "\\Service Authorizations");
            return true;
        } catch (Exception ex) when (
              ex is IOException ||
              ex is UnauthorizedAccessException ||
              ex is DirectoryNotFoundException
          ) {
            error = ex.Message;
            return false;
        }
    }
}