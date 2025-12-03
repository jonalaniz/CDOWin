namespace CDO.Core.DTOs;

public class ClientSummaryDTO {
    // Non-optional fields
    public int id { get; init; }
    public string firstName { get; init; }
    public string lastName { get; init; }
    public string city { get; init; }
    public string state { get; init; }

    // Nullable fields
    public string? address1 { get; init; }
    public string? address2 { get; init; }
    public string? zip { get; init; }
    public string? counselorName { get; init; }

    // Computed Properties
    public string name => $"{lastName}, {firstName}";
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
