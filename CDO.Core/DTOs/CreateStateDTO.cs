namespace CDO.Core.DTOs;

public class CreateStateDTO {
    // Required creation fields
    public required int id { get; init; }
    public required string name { get; init; }

    // Optional fields
    public int? countryID { get; init; }
    public string? shortName { get; init; }
}