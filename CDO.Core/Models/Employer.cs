namespace CDO.Core.Models;

public record class Employer(
    int id,
    string? name,
    string? address1,
    string? address2,
    string? city,
    string? state,
    string? zip,
    string? phone,
    string? fax,
    string? email,
    string? website,
    string? notes,
    string? supervisor,
    string? supervisorPhone,
    string? supervisorEmail
    ) {
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

    public string formattedCityStateZip {
        get {
            if (zip != null)
                return $"{city}, {state} {zip}";
            else
                return $"{city}, {state}";
        }
    }
}
