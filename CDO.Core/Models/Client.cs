using CDO.Core.DTOs;

namespace CDO.Core.Models;

public record class Client(
    int Id,
    string FirstName,
    string LastName,
    bool TTW,
    Reminder[] Reminders,
    Placement[]? Placements,
    Invoice[]? Invoices,
    DateTime? StartDate,
    int? Ssn,
    string? CaseID,
    string? Address1,
    string? Address2,
    string City,
    string State,
    string? Zip,
    DateTime? Dob,
    string? DriversLicense,
    string? Phone1,
    string? Phone1Identity,
    string? Phone2,
    string? Phone2Identity,
    string? Phone3,
    string? Phone3Identity,
    string? Email,
    string? EmailIdentity,
    string? Email2,
    string? Email2Identity,
    string Disability,
    int? CounselorID,
    string? ClientNotes,
    string? Conditions,
    string? DocumentFolder,
    bool? Active,
    string? EmploymentGoal,
    int? EmployerID,
    string? Status,
    string? Benefits,
    string? CriminalCharge,
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
    string? Race,
    string? FluentLanguages,
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
