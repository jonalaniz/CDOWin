namespace CDO.Core.Models;

public record class Employer(
    int Id,
    string? Name,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? Zip,
    string? Phone,
    string? Fax,
    string? Email,
    string? Website,
    string? Notes,
    string? SupervisorName,
    string? SupervisorPhone,
    string? SupervisorEmail
    ) {

    public string? FormattedSuperviror {
        get {
            var text = $"{SupervisorName}\n{SupervisorPhone}\n{SupervisorEmail}";
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            return text;
        }
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
            if (Zip != null)
                return $"{City}, {State} {Zip}";
            else
                return $"{City}, {State}";
        }
    }
}
