namespace CDO.Core.DTOs.Clients;

public record class ClientSummary(
    // Non-optional fields
    int Id,
    string FirstName,
    string LastName,
    string City,
    string State,
    bool Active,
    bool TTW,

    // Nullable fields
    string? Address1,
    string? Address2,
    string? Zip,
    string? CounselorName,
    string? Phone,
    string? Phone2,
    string? Phone3,
    string? EmploymentGoal,
    string? CaseID
    ) {

    // Computed Properties
    public string Name => $"{LastName}, {FirstName}";
    public bool InActive => !Active;
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
            if (Zip != null) return $"{City}, {State} {Zip}";
            else return $"{City}, {State}";
        }
    }
}
