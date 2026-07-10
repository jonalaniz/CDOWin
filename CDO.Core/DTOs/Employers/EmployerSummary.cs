namespace CDO.Core.DTOs.Employers;

public record class EmployerSummary(
    // Non-optional fields
    int Id,

    // Nullable fields
    string? Name,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? Zip,
    string? Phone,
    string? Website,
    string? Notes,
    string? SupervisorName,
    string? SupervisorPhone
    ) {

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

    public string? FormattedSupervisor {
        get {
            var text = $"{SupervisorName}\n{SupervisorPhone}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
}
