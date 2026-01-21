using CDO.Core.DTOs;

namespace CDO.Core.Models;

public record class Client(
    int Id,
    string FirstName,                                       // FirstName
    string LastName,                                        // LastName
    string Counselor,
    Reminder[] Reminders,
    Placement[]? Placements,
    Invoice[]? Invoice,
    DateTime? StartDate,
    int? Ssn,                                               // SSN
    string? CaseID,
    string? Address1,                                       // Address1
    string? Address2,                                       // Address2
    string City,                                            // City
    string State,                                           // State
    string? Zip,                                            // Zip
    DateTime? Dob,                                          // DOB
    string? DriversLicense,                                 // DL
    string? Phone1,                                         // Phone1
    string? Phone1Identity,
    string? Phone2,                                         // Phone2
    string? Phone2Identity,
    string? Phone3,                                         // Phone3
    string? Phone3Identity,
    string? Email,                                          // Email
    string? EmailIdentity,
    string? Email2,
    string? Email2Identity,
    string Disability,
    int? CounselorID,
    string? CounselorEmail,
    string? CounselorPhone,
    string? CounselorFax,
    string? ClientNotes,
    string? Conditions,
    string? DocumentFolder,
    bool? Active,
    string? EmploymentGoal,
    int? EmployerID,
    string? Status,
    string? Benefits,
    string? CriminalCharge,                                 // NOCCCHK If (!empty) CriminalCharge
    string? Education,
    string? Transportation,
    bool? ResumeRequired,
    bool? ResumeCompleted,
    bool? VideoInterviewRequired,
    bool? VideoInterviewCompleted,
    bool? ReleasesCompleted,
    bool? OrientationCompleted,
    bool? DataSheetCompleted,
    bool? ElevatorSpeechCompleted,
    string? Race,                                           // Race
    string? FluentLanguages,                                // ECK/SCK/OLCK | OtherLang
    string? Premiums,
    UpdateCounselorDTO? CounselorReference
    ) {
    public string NameAndID => $"{FirstName} {LastName} ({Id})";
    public string FormattedName => $"{FirstName} {LastName}";

    public string? DocumentsFolderPath => DocumentFolder?.Replace('#', ' ').Trim();

    public string FormattedAddress {
        get {
            if (Address1 == null && Address2 == null)
                return "No address on file.";
            else if (Address2 == null) {
                return $"{Address1}\n{FormattedCityStateZip}";
            } else {
                return $"{Address1} {Address2}\n{FormattedCityStateZip}";
            }
        }
    }

    public string? FormattedDOB => Dob?.ToString(format: "MM/dd/yyyy");

    public string FormattedCityStateZip {
        get {
            if (Zip != null)
                return $"{City}, {State} {Zip}";
            else
                return $"{City}, {State}";
        }
    }

    public string FormattedSSN {
        get {
            if (Ssn.ToString() is string unwrappedSSN) {
                if (unwrappedSSN.Length == 9) {
                    return $"{unwrappedSSN.Substring(0, 3)}-{unwrappedSSN.Substring(3, 2)}-{unwrappedSSN.Substring(5, 4)}";
                }
            }
            return "";
        }
    }

    public string? FormattedStartDate => StartDate?.ToString(format: "MM/dd/yyyy");
}
