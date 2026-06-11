namespace CDO.Core.DTOs.Admin;

public class AdminClientSummary {
    // Non-optional fields
    public int Id { get; init; }
    public required bool Active { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }
    public required bool Ttw { get; init; }

    // Nullable fields
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string? Address1 { get; init; }
    public string? Address2 { get; init; }
    public string? Zip { get; init; }
    public string? CaseID { get; init; }

    // Computed Properties
    public string Name => $"{FirstName} {LastName}";
    public string FormattedID => $"ID: {Id}";
    public string FormattedCreatedDate => CreatedAt?.ToString(format: "MM/dd/yyyy") ?? "No Date On File";
    public string FormattedUpdatedDate => UpdatedAt?.ToString(format: "MM/dd/yyyy") ?? "No Date On File";
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
