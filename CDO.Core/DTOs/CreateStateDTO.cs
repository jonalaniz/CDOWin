namespace CDO.Core.DTOs;

public class CreateStateDTO {
    // Required creation fields
    public required int Id { get; init; }
    public required string Name { get; init; }

    // Optional fields
    public int? CountryID { get; init; }
    public string? ShortName { get; init; }
}