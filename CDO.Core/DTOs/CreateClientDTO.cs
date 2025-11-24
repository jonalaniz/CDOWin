namespace CDO.Core.DTOs;

public class CreateClientDTO {
    // Required creation fields
    public required string firstName { get; init; }
    public required string lastName { get; init; }
    public required string counselor { get; init; }
    public required string city { get; init; }
    public required string state { get; init; }
    public required string disability { get; init; }


    // Optional fields
    public int? ssn { get; init; }
    public string? caseID { get; init; }
    public string? address1 { get; init; }
    public string? address2 { get; init; }
    public string? zip { get; init; }
    public DateTime? dob { get; init; }
    public DateTime? startDate { get; init; }
    public string? driversLicense { get; init; }
    public string? phone1 { get; init; }
    public string? phone1Identity { get; init; }
    public string? phone2 { get; init; }
    public string? phone2Identity { get; init; }
    public string? phone3 { get; init; }
    public string? phone3Identity { get; init; }
    public string? email { get; init; }
    public string? emailIdentity { get; init; }
    public string? email2 { get; init; }
    public string? email2Identity { get; init; }
    public int? counselorID { get; init; }
    public string? counselorEmail { get; init; }
    public string? counselorPhone { get; init; }
    public string? counselorFax { get; init; }
    public string? clientNotes { get; init; }
    public string? conditions { get; init; }
    public string? documentFolder { get; init; }
    public bool? active { get; init; }
    public string? employmentGoal { get; init; }
    public int? employerID { get; init; }
    public string? status { get; init; }
    public string? benefit { get; init; }
    public string? criminalCharge { get; init; }
    public string? education { get; init; }
    public string? transportation { get; init; }
    public bool? resumeRequired { get; init; }
    public bool? resumeCompleted { get; init; }
    public bool? videoInterviewRequired { get; init; }
    public bool? videoInterviewCompleted { get; init; }
    public bool? releasesCompleted { get; init; }
    public bool? orientationCompleted { get; init; }
    public bool? dataSheetCompleted { get; init; }
    public bool? elevatorSpeechCompleted { get; init; }
    public string? race { get; init; }
    public string? fluentLanguages { get; init; }
    public string? premium { get; init; }
}