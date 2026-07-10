namespace CDO.Core.DTOs.Admin;

public record class ClientHistory(
    int Id,
    bool Active,
    string FirstName,
    string LastName,
    string City,
    string State,
    bool Ttw,
    ClientActivity[] Activities,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string? Address1,
    string? Address2,
    string? Zip,
    string? CaseID
    ) {

    // Computed Properties
    public string Name => $"{FirstName} {LastName}";
    public bool InActive => !Active;
    public string FormattedID => $"ID: {Id}";
    public string FormattedCreatedDate => CreatedAt.ToLocalTime().ToString(format: "MM/dd/yyyy") ?? "No Date On File";
    public string FormattedUpdatedDate => UpdatedAt.ToLocalTime().ToString(format: "MM/dd/yyyy") ?? "No Date On File";
    public string FormattedUpdatedAtTime() {
        if (Time(UpdatedAt) is not string time) return "No Time On File";
        return $"Updated at: {time}";
    }

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

    private string? Time(DateTime date) {
        return date.ToLocalTime().ToString(format: "hh:mm tt");
    }

    // Convenience Methods
    public AdminClientSummary ToggleActive() {
        return new AdminClientSummary(
            Id,
            !Active,
            FirstName,
            LastName,
            City,
            State,
            Ttw,
            CreatedAt,
            UpdatedAt,
            Address1,
            Address2,
            Zip,
            CaseID
            );
    }

    public AdminClientSummary ToggleTTW() {
        return new AdminClientSummary(
            Id,
            Active,
            FirstName,
            LastName,
            City,
            State,
            !Ttw,
            CreatedAt,
            UpdatedAt,
            Address1,
            Address2,
            Zip,
            CaseID
            );
    }
}