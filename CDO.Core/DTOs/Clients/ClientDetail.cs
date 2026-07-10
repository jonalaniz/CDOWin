using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.DTOs.Counselors;
using CDO.Core.DTOs.Placements;
using CDO.Core.DTOs.SAs;
using CDO.Core.Models;

namespace CDO.Core.DTOs.Clients;

public record class ClientDetail(
    int Id,

    // Required Fields
    string FirstName,
    string LastName,
    bool Active,
    int? CounselorID,
    bool TTW,

    // Parent Object
    CounselorUpdate? CounselorReference,

    // Child Objects
    Reminder[] Reminders,
    SADetail[]? Sas,
    PlacementDetail[]? Placements,
    ClientNote[]? ClientNotes,

    DateTime? StartDate,
    string? Ssn,
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
    string? Conditions,
    string? DocumentFolder,
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
    string? Premiums
    ) {
    public bool InActive => !Active;
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

    public string FormattedStreetAddress {
        get {
            if (Address1 == null && Address2 == null)
                return "No street address on file.";
            else if (Address2 == null) {
                return Address1 ?? "No street address on file.";
            } else {
                return $"{Address1} {Address2}";
            }
        }
    }

    public string? FormattedDOB => Dob?.ToString(format: "MM/dd/yyyy");

    public string FormattedCityStateZip {
        get {
            if (Zip != null) return $"{City}, {State} {Zip}";
            else return $"{City}, {State}";
        }
    }

    public string FormattedSSN {
        get {
            if (Ssn is string unwrappedSSN) {
                if (unwrappedSSN.Length == 9) {
                    return $"{unwrappedSSN.Substring(0, 3)}-{unwrappedSSN.Substring(3, 2)}-{unwrappedSSN.Substring(5, 4)}";
                }
            }
            return "";
        }
    }

    public string? FormattedStartDate => StartDate?.ToString(format: "MM/dd/yyyy");

    public ClientSummary AsSummary() {
        return new ClientSummary(
            Id,
            FirstName,
            LastName,
            City,
            State,
            Active,
            TTW,
            Address1,
            Address2,
            Zip,
            CounselorReference?.Name,
            Phone1,
            Phone2,
            Phone3,
            EmploymentGoal,
            CaseID
            );
    }

    public ClientExport AsExport() {
        return new ClientExport(
            Id,
            FirstName,
            LastName,
            FormattedDOB,
            DriversLicense,
            Ssn,
            City,
            State,
            Active,
            TTW,
            CaseID,
            Address1,
            Address2,
            Zip,
            CounselorReference?.Name,
            Phone1,
            Phone2,
            Phone3,
            Email,
            EmailIdentity,
            Email2,
            Email2Identity,
            EmploymentGoal,
            Sas?.Select(i => i.AsExport()).ToArray()
            );
    }
}