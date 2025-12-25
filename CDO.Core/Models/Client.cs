using CDO.Core.DTOs;

namespace CDO.Core.Models;

public record class Client(
    int id,
    string firstName,
    string lastName,
    string counselor,
    Reminder[] reminders,
    Placement[]? placements,
    ServiceAuthorization[]? pos,
    DateTime? startDate,
    int? ssn,
    string? caseID,
    string? address1,
    string? address2,
    string city,
    string state,
    string? zip,
    DateTime? dob,
    string? driversLicense,
    string? phone1,
    string? phone1Identity,
    string? phone2,
    string? phone2Identity,
    string? phone3,
    string? phone3Identity,
    string? email,
    string? emailIdentity,
    string? email2,
    string? email2Identity,
    string disability,
    int? counselorID,
    string? counselorEmail,
    string? counselorPhone,
    string? counselorFax,
    string? clientNotes,
    string? conditions,
    string? documentFolder,
    bool? active,
    string? employmentGoal,
    int? employerID,
    string? status,
    string? benefits,
    string? criminalCharge,
    string? education,
    string? transportation,
    bool? resumeRequired,
    bool? resumeCompleted,
    bool? videoInterviewRequired,
    bool? videoInterviewCompleted,
    bool? releasesCompleted,
    bool? orientationCompleted,
    bool? dataSheetCompleted,
    bool? elevatorSpeechCompleted,
    string? race,
    string? fluentLanguages,
    string? premiums,
    UpdateCounselorDTO? counselorReference
    ) {
    public string name => $"{firstName} {lastName} ({id})";

    public string? documentsFolderPath => documentFolder?.Replace('#', ' ').Trim();

    public string formattedAddress {
        get {
            if (address1 == null && address2 == null)
                return "No address on file.";
            else if (address2 == null) {
                return $"{address1}\n{formattedCityStateZip}";
            } else {
                return $"{address1} {address2}\n{formattedCityStateZip}";
            }
        }
    }

    public string? formattedDOB => dob?.ToString(format: "MM/dd/yyyy");

    public string formattedCityStateZip {
        get {
            if (zip != null)
                return $"{city}, {state} {zip}";
            else
                return $"{city}, {state}";
        }
    }

    public string formattedSSN {
        get {
            if (ssn.ToString() is { } unwrappedSSN) {
                if (unwrappedSSN.Length == 9) {
                    return $"{unwrappedSSN.Substring(0, 3)}-{unwrappedSSN.Substring(3, 2)}-{unwrappedSSN.Substring(5, 4)}";
                }
            }
            return ssn.ToString();
        }
    }

    public string? formattedStartDate => startDate?.ToString(format: "MM/dd/yyyy");
}
