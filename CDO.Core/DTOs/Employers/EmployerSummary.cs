namespace CDO.Core.DTOs.Employers;

public class EmployerSummary {
    // Non-optional fields
    public int Id { get; init; }

    // Nullable fields
    public string? Name { get; init; }
    public string? Address1 { get; init; }
    public string? Address2 { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Zip { get; init; }
    public string? Phone { get; init; }
    public string? Website { get; init; }
    public string? Notes { get; init; }
    public string? Supervisor { get; init; }
    public string? SupervisorPhone { get; init; }

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
            var text = $"{Supervisor}\n{SupervisorPhone}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
    }
}
