namespace CDO.Core.DTOs.Admin;

public class AdminClientDetail {
    // Non-optional fields
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required bool Active { get; init; }
    public required bool Ttw { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }
    public required string Disability { get; init; }

    // Nullable fields
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? StartDate { get; init; }
    public int? CounselorID { get; init; }
    public string? Ssn { get; init; }
    public string? CaseID { get; init; }
    public string? Address1 { get; init; }
    public string? Address2 { get; init; }
    public string? Zip { get; init; }
    public DateTime? Dob { get; init; }
    public string? DriversLicense { get; init; }
    public string? Phone1 { get; init; }
    public string? Phone1Identity { get; init; }
    public string? Phone2 { get; init; }
    public string? Phone2Identity { get; init; }
    public string? Phone3 { get; init; }
    public string? Phone3Identity { get; init; }
    public string? Email { get; init; }
    public string? EmailIdentity { get; init; }
    public string? Email2 { get; init; }
    public string? Email2Identity { get; init; }
    public string? Conditions { get; init; }
    public string? EmploymentGoal { get; init; }
    public string? Status { get; init; }
    public string? Benefits { get; init; }
    public string? CriminalCharge { get; init; }
    public string? Education { get; init; }
    public string? Transportation { get; init; }
    public bool? ResumeRequired { get; init; }
    public bool? ResumeCompleted { get; init; }
    public bool? VideoInterviewRequired { get; init; }
    public bool? VideoInterviewCompleted { get; init; }
    public bool? ReleasesCompleted { get; init; }
    public bool? OrientationCompleted { get; init; }
    public bool? DataSheetCompleted { get; init; }
    public bool? ElevatorSpeechCompleted { get; init; }
    public string? Race { get; init; }
    public string? FluentLanguages { get; init; }
    public string? Premiums { get; init; }
}