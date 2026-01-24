namespace CDO.Core.DTOs;

public class ClientSummaryDTO {
    // Non-optional fields
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }

    public required bool TTW { get; init; }

    // Nullable fields
    public string? Address1 { get; init; }
    public string? Address2 { get; init; }
    public string? Zip { get; init; }
    public string? CounselorName { get; init; }
    public string? Phone { get; init; }
    public string? Phone2 { get; init; }
    public string? Phone3 { get; init; }
    public string? EmploymentGoal { get; init; }
    public string? CaseID { get; init; }


    // Computed Properties
    public string Name => $"{LastName}, {FirstName}";
    public string FormattedName => $"{FirstName} {LastName}";
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

    public string FormattedCityStateZip {
        get {
            if (Zip != null)
                return $"{City}, {State} {Zip}";
            else
                return $"{City}, {State}";
        }
    }
}
